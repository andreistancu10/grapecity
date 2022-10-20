using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IPerformanceIndicatorFunctionaryService
    {
        Task AddRangeAsync(long id, List<long> functionaryIds, CancellationToken cancellationToken);
        Task UpdateRangeAsync(long id, List<long> functionaryIds, CancellationToken cancellationToken);
        IQueryable<PerformanceIndicatorFunctionary> FindByIdQuery(long performanceIndicatorId);
    }
    public class PerformanceIndicatorFunctionaryService : IPerformanceIndicatorFunctionaryService
    {
        private readonly DocumentManagementDbContext _dbContext;
        public PerformanceIndicatorFunctionaryService(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddRangeAsync(long id, List<long> functionaryIds, CancellationToken cancellationToken)
        {
            if (!functionaryIds.Any()) return;

            var performanceIndicatorFunctionaryToInsert = new List<PerformanceIndicatorFunctionary>();
            foreach (var functionaryId in functionaryIds)
            {
                performanceIndicatorFunctionaryToInsert.Add(
                    new PerformanceIndicatorFunctionary()
                    {
                        PerformanceIndicatorId = id,
                        FunctionaryId = functionaryId,
                    });
            }
            await _dbContext.AddRangeAsync(performanceIndicatorFunctionaryToInsert, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<PerformanceIndicatorFunctionary> FindByIdQuery(long performanceIndicatorId)
        {
            return _dbContext.PerformanceIndicatorFunctionaries.Where(x => x.PerformanceIndicatorId == performanceIndicatorId).AsQueryable();
        }

        public async Task UpdateRangeAsync(long id, List<long> functionaryIds, CancellationToken cancellationToken)
        {
            if (!functionaryIds.Any()) return;

            var initialPerformanceIndicatorFunctionary = await FindByIdQuery(id).ToListAsync(cancellationToken);

            if (initialPerformanceIndicatorFunctionary.Count > 0)
            {
                var initialFunctionaryIds = initialPerformanceIndicatorFunctionary.Select(x => x.FunctionaryId).ToList();
                var functionaryIdsToRemove = initialFunctionaryIds.Except(functionaryIds);

                if (functionaryIdsToRemove.Any())
                {
                    var itemsToDelete = initialPerformanceIndicatorFunctionary.Where(x => functionaryIdsToRemove.Any(y => y == x.FunctionaryId)).ToArray();
                    _dbContext.PerformanceIndicatorFunctionaries.RemoveRange(itemsToDelete);
                }
                functionaryIds = functionaryIds.Except(initialFunctionaryIds).ToList();
            }

            var performanceIndicatorFunctionaryToInsert = new List<PerformanceIndicatorFunctionary>();
            foreach (var procedureFunctionaryId in functionaryIds)
            {
                performanceIndicatorFunctionaryToInsert.Add(
                    new PerformanceIndicatorFunctionary()
                    {
                        PerformanceIndicatorId = id,
                        FunctionaryId = procedureFunctionaryId,
                    });
            }
            await _dbContext.AddRangeAsync(performanceIndicatorFunctionaryToInsert, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
