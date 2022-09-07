using DigitNow.Domain.DocumentManagement.Business.Actions.Queries.FilterActions;
using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Infrastructure.Data.Abstractions;
using HTSS.Platform.Infrastructure.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Action = DigitNow.Domain.DocumentManagement.Data.Entities.Action;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
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
            var descriptor = new FilterDescriptor<Action>(_dbContext.Actions.Include(c => c.AssociatedActivity).AsNoTracking(), request.SortField, request.SortOrder);
            descriptor.Query(p => request.DepartmentIds.Contains(p.DepartmentId));
            descriptor.Query(p => p.ActivityId == request.ActivityId);
            var pagedResult = await descriptor.PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);

            return pagedResult;
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
                .Where(item => item.CreatedAt.Year == DateTime.UtcNow.Year && item.ActivityId == action.ActivityId)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync(token);

            var order = lastAction?.Id ?? 0;
            var activity = await _dbContext.Activities
                .FirstOrDefaultAsync(x => x.Id == action.ActivityId, token);

            if (activity != null)
            {
                action.Code = $"ACT{DateTime.Now.Year}_{activity.GeneralObjectiveId}.{activity.SpecificObjectiveId}.{activity.Id}.{++order}";
            }
        }
    }
}
