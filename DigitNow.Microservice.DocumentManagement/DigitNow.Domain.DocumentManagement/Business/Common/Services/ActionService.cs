using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IActionService
    {
        Task<Data.Entities.Actions.Action> CreateAsync(Data.Entities.Actions.Action action, CancellationToken cancellationToken);
        Task UpdateAsync(Data.Entities.Actions.Action action, CancellationToken cancellationToken);
        IQueryable<Data.Entities.Actions.Action> FindQuery();
    }
    public class ActionService : IActionService
    {
        private readonly DocumentManagementDbContext _dbContext;

        public ActionService(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Data.Entities.Actions.Action> CreateAsync(Data.Entities.Actions.Action action, CancellationToken cancellationToken)
        {
            action.State = ScimState.Active;
            await SetActionCodeAsync(action, cancellationToken);
            await _dbContext.Actions.AddAsync(action, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return action;
        }

        public async Task UpdateAsync(Data.Entities.Actions.Action action, CancellationToken cancellationToken)
        {
            await _dbContext.SingleUpdateAsync(action, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<Data.Entities.Actions.Action> FindQuery()
        {
            return _dbContext.Actions.AsQueryable();
        }


        private async Task SetActionCodeAsync(Data.Entities.Actions.Action action, CancellationToken token)
        {
            var lastAction = await _dbContext.Actions
                .Where(item => item.CreatedAt.Year == DateTime.UtcNow.Year && item.ActivityId == action.ActivityId)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync(token);

            var order = lastAction != null ? lastAction.Id : 0;
            var activity = await _dbContext.Activities
                .FirstOrDefaultAsync(x => x.Id == action.ActivityId);

            if (activity != null)
            {
                action.Code = $"ACT{DateTime.Now.Year}_{activity.GeneralObjectiveId}.{activity.SpecificObjectiveId}.{activity.Id}.{++order}";
            }
        }
    }
}
