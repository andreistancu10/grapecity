using DigitNow.Domain.DocumentManagement.Business.Actions.Queries.FilterActions;
using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Infrastructure.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using Action = DigitNow.Domain.DocumentManagement.Data.Entities.Action;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    //TODO: Refactor this (Exclude query from parameters)
    public interface IActionService : IScimStateService
    {
        Task<PagedList<Action>> GetActionsAsync(FilterActionsQuery request, CancellationToken cancellationToken);
        Task<Action> CreateAsync(Action action, CancellationToken cancellationToken);
        Task UpdateAsync(Action action, CancellationToken cancellationToken);
        IQueryable<Action> FindQuery();
    }

    public class ActionService : ScimStateService, IActionService
    {
        public ActionService(DocumentManagementDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PagedList<Action>> GetActionsAsync(FilterActionsQuery request, CancellationToken cancellationToken)
        {
            var actions = await DbContext.Actions
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
                .Where(item => item.CreatedAt.Year == DateTime.UtcNow.Year && item.ActivityId == action.ActivityId)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync(token);

            var order = lastAction?.Id ?? 0;
            var activity = await DbContext.Activities
                .FirstOrDefaultAsync(x => x.Id == action.ActivityId, token);

            if (activity != null)
            {
                action.Code = $"ACT{DateTime.Now.Year}_{activity.GeneralObjectiveId}.{activity.SpecificObjectiveId}.{activity.Id}.{++order}";
            }
        }
    }
}
