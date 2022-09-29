using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IActivityFunctionaryService
    {
        Task AddRangeAsync(long id, List<long> functionaryIds, CancellationToken cancellationToken);
        Task UpdateRangeAsync(long id, List<long> functionaryIds, CancellationToken cancellationToken);
        IQueryable<ActivityFunctionary> FindQuery();
    }
    public class ActivityFunctionaryService : IActivityFunctionaryService
    {
        private readonly DocumentManagementDbContext _dbContext;

        public ActivityFunctionaryService(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddRangeAsync(long id, List<long> functionaryIds, CancellationToken cancellationToken)
        {
            if (!functionaryIds.Any()) return;

            var ActivityFunctionaryToInsert = new List<ActivityFunctionary>();
            foreach (var functionaryId in functionaryIds)
            {
                ActivityFunctionaryToInsert.Add(
                    new ActivityFunctionary()
                    {
                        ActivityId = id,
                        FunctionaryId = functionaryId,
                    });
            }
            await _dbContext.AddRangeAsync(ActivityFunctionaryToInsert, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<ActivityFunctionary> FindQuery()
        {
            return _dbContext.ActivityFunctionaries.AsQueryable();
        }

        public async Task UpdateRangeAsync(long id, List<long> functionaryIds, CancellationToken cancellationToken)
        {
            if (!functionaryIds.Any()) return;

            var initialActivityFunctionary = await FindQuery().Where(item => item.ActivityId == id).ToListAsync(cancellationToken);

            if (initialActivityFunctionary.Count != 0)
            {
                var initialFunctionaryIds = initialActivityFunctionary.Select(x => x.FunctionaryId).ToList();
                var functionaryIdsToRemove = initialFunctionaryIds.Except(functionaryIds);

                if (functionaryIdsToRemove.Any())
                {
                    var itemsToDelete = initialActivityFunctionary.Where(x => functionaryIdsToRemove.Any(y => y == x.FunctionaryId)).ToArray();
                    _dbContext.ActivityFunctionaries.RemoveRange(itemsToDelete);
                }
                functionaryIds = functionaryIds.Except(initialFunctionaryIds).ToList();
            }

            var ActivityFunctionaryToInsert = new List<ActivityFunctionary>();
            foreach (var ActivityFunctionaryId in functionaryIds)
            {
                ActivityFunctionaryToInsert.Add(
                    new ActivityFunctionary()
                    {
                        ActivityId = id,
                        FunctionaryId = ActivityFunctionaryId,
                    });
            }
            await _dbContext.AddRangeAsync(ActivityFunctionaryToInsert, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
