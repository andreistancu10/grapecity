using DigitNow.Domain.DocumentManagement.Business.Actions.Queries.FilterActions;
using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Infrastructure.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Action = DigitNow.Domain.DocumentManagement.Data.Entities.Action;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    //TODO: Refactor this (Exclude query from parameters)
    public interface IActionService
    {
        Task<PagedList<Action>> GetActionsAsync(FilterActionsQuery request, CancellationToken cancellationToken);
        Task<Action> CreateAsync(Action action, CancellationToken cancellationToken);
        Task UpdateAsync(Action action, CancellationToken cancellationToken);
        IQueryable<Action> FindQuery();
    }

    public class ActionService : IActionService
    {
        private readonly DocumentManagementDbContext _dbContext;

        public ActionService(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<Action>> GetActionsAsync(FilterActionsQuery request, CancellationToken cancellationToken)
        {
            var actions = await _dbContext.Actions
                .Include(x => x.AssociatedActivity)
                .Where(x => x.ActivityId == request.ActivityId)
                .Skip((int)(request.PageSize * (request.PageNumber - 1)))
                .Take((int)request.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedList<Action>(actions.AsQueryable(), request.PageNumber, request.PageSize);
        }

        public async Task<Action> CreateAsync(Action action, CancellationToken cancellationToken)
        {
            action.State = ScimState.Active;
            await SetActionCodeAsync(action, cancellationToken);
            await _dbContext.Actions.AddAsync(action, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return action;
        }

        public async Task UpdateAsync(Action action, CancellationToken cancellationToken)
        {
            await _dbContext.SingleUpdateAsync(action, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<Action> FindQuery()
        {
            return _dbContext.Actions.AsQueryable();
        }

        private async Task SetActionCodeAsync(Action action, CancellationToken token)
        {
            var lastAction = await _dbContext.Actions
                     .Where(item => item.ActivityId == action.ActivityId)
                     .OrderByDescending(p => p.CreatedAt)
                     .FirstOrDefaultAsync(token);

            var sb = new StringBuilder();
            if (lastAction == null)
            {
                var activity = await _dbContext.Activities
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
