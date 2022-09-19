using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Contracts.Scim;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Infrastructure.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Text;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Actions;
using DigitNow.Domain.DocumentManagement.Data.Filters.Actions;
using Action = DigitNow.Domain.DocumentManagement.Data.Entities.Action;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    //TODO: Refactor this (Exclude query from parameters)
    public interface IActionService : IScimStateService
    {
        Task<PagedList<Action>> GetActionsAsync(ActionsFilter filter, CancellationToken cancellationToken);
        Task<Action> CreateAsync(Action action, CancellationToken cancellationToken);
        Task UpdateAsync(Action action, CancellationToken cancellationToken);
        IQueryable<Action> FindQuery();
    }

    public class ActionService : ScimStateService, IActionService
    {
        public ActionService(DocumentManagementDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PagedList<Action>> GetActionsAsync(ActionsFilter filter, CancellationToken cancellationToken)
        {
            var actions = await DbContext.Actions
                .Include(x => x.AssociatedActivity)
                .Where(x => x.ActivityId == filter.ActivityId)
                .Skip((int)((filter.PageSize ?? 20) * (filter.PageNumber - 1)))
                .Take(filter.PageSize ?? 20)
                .ToListAsync(cancellationToken);

            return new PagedList<Action>(actions.AsQueryable(), filter.PageNumber, filter.PageSize);
        }

        public async Task<Action> CreateAsync(Action action, CancellationToken cancellationToken)
        {
            action.State = ScimState.Active;
            await SetActionCodeAsync(action, cancellationToken);
            await DbContext.Actions.AddAsync(action, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);

            return action;
        }

        public async Task UpdateAsync(Action action, CancellationToken cancellationToken)
        {
            await ChangeStateAsync(new List<long> { action.Id }, ScimEntity.ScimAction, action.State, cancellationToken);
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
    }
}
