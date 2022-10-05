using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Contracts.Scim;
using DigitNow.Domain.DocumentManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Actions;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Actions;
using Action = DigitNow.Domain.DocumentManagement.Data.Entities.Action;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IActionService : IScimStateService
    {
        Task<List<Action>> GetActionsAsync(ActionFilter filter, UserModel currentUser, int page, int count,
            CancellationToken cancellationToken);
        Task<Action> CreateAsync(Action action, CancellationToken cancellationToken);
        Task UpdateAsync(Action action, CancellationToken cancellationToken);
        IQueryable<Action> FindQuery();
    }

    public class ActionService : ScimStateService, IActionService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICatalogClient _catalogClient;

        public ActionService(
            DocumentManagementDbContext dbContext,
            IServiceProvider serviceProvider,
            ICatalogClient catalogClient) : base(dbContext)
        {
            _serviceProvider = serviceProvider;
            _catalogClient = catalogClient;
        }

        public async Task<List<Action>> GetActionsAsync(ActionFilter filter, UserModel currentUser, int page, int count, CancellationToken cancellationToken)
        {
            return await GetBuiltInActionsQuery()
                .WhereAll((await GetActionsDataExpressions(filter, currentUser, cancellationToken)).ToPredicates())
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * count)
                .Take(count)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Action> CreateAsync(Action action, CancellationToken cancellationToken)
        {
            var activeScimState = await _catalogClient.ScimStates.GetScimStateByCodeAsync("activ", cancellationToken);
            action.StateId = activeScimState.Id;

            await SetActionCodeAsync(action, cancellationToken);
            await DbContext.Actions.AddAsync(action, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);

            return action;
        }

        public async Task UpdateAsync(Action action, CancellationToken cancellationToken)
        {
            await ChangeStateAsync(new List<long> { action.Id }, ScimEntity.ScimAction, action.StateId, cancellationToken);
            await DbContext.SingleUpdateAsync(action, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<Action> FindQuery()
        {
            return DbContext.Actions.AsQueryable();
        }

        private async Task SetActionCodeAsync(Action action, CancellationToken token)
        {
            var lastAction = await DbContext.Actions
                                 .Where(item => item.ActivityId == action.ActivityId)
                                 .OrderByDescending(p => p.CreatedAt)
                                 .FirstOrDefaultAsync(token);

            var sb = new StringBuilder();

            if (lastAction == null)
            {
                var activity = await DbContext.Activities
                    .Where(item => item.Id == action.ActivityId)
                    .FirstOrDefaultAsync(token);

                sb.Append(activity.Code.Replace("ACTIV", "ACT"));
                sb.Append(".1");
            }
            else
            {
                string[] subs = lastAction.Code.Split('.');
                sb.Append(subs[0]);
                sb.Append('.');
                sb.Append(subs[1]);
                sb.Append('.');
                sb.Append(subs[2]);
                sb.Append('.');
                sb.Append(int.Parse(subs[3]) + 1);
            }
            action.Code = sb.ToString();
        }

        private Task<DataExpressions<Action>> GetActionsDataExpressions(
            ActionFilter filter,
            UserModel currentUser,
            CancellationToken token)
        {
            var actionsFilterComponent = new ActionsFilterComponent(_serviceProvider);
            var actionsFilterComponentContext = new ActionsFilterComponentContext
            {
                CurrentUser = currentUser,
                ActionFilter = filter
            };

            return actionsFilterComponent.ExtractDataExpressionsAsync(actionsFilterComponentContext, token);
        }

        private IQueryable<Action> GetBuiltInActionsQuery()
        {
            return DbContext.Actions
                .Include(c => c.ActionFunctionaries)
                .Include(x => x.AssociatedActivity);
        }
    }
}
