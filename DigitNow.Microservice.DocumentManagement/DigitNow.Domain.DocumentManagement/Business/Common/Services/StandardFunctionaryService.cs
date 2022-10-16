using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IStandardFunctionaryService
    {
        Task AddRangeAsync(long standardId, List<long> functionaryIds, CancellationToken cancellationToken);
        Task UpdateRangeAsync(long standardId, List<long> functionaryIds, CancellationToken cancellationToken);
    }
    public class StandardFunctionaryService: IStandardFunctionaryService
    {
        private readonly DocumentManagementDbContext _dbContext;
        public StandardFunctionaryService(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRangeAsync(long standardId, List<long> functionaryIds, CancellationToken cancellationToken)
        {
            if (!functionaryIds.Any()) return;

            var standardFunctionaryToInsert = new List<StandardFunctionary>();
            foreach (var functionaryId in functionaryIds)
            {
                standardFunctionaryToInsert.Add(
                    new StandardFunctionary()
                    {
                        StandardId = standardId,
                        FunctionaryId = functionaryId,
                    });
            }
            await _dbContext.AddRangeAsync(standardFunctionaryToInsert, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateRangeAsync(long standardId, List<long> functionaryIds, CancellationToken cancellationToken)
        {
            if (!functionaryIds.Any()) return;

            var initialStandardFunctionary = await FindQuery().Where(item => item.StandardId == standardId).ToListAsync(cancellationToken);

            if (initialStandardFunctionary.Count != 0)
            {
                var initialFunctionaryIds = initialStandardFunctionary.Select(x => x.FunctionaryId).ToList();
                var functionaryIdsToRemove = initialFunctionaryIds.Except(functionaryIds);

                if (functionaryIdsToRemove.Any())
                {
                    var itemsToDelete = initialStandardFunctionary.Where(x => functionaryIdsToRemove.Any(y => y == x.FunctionaryId)).ToArray();
                    _dbContext.StandardFunctionaries.RemoveRange(itemsToDelete);
                }
                functionaryIds = functionaryIds.Except(initialFunctionaryIds).ToList();
            }

            var standardFunctionaryToInsert = new List<StandardFunctionary>();
            foreach (var standardFunctionaryId in functionaryIds)
            {
                standardFunctionaryToInsert.Add(
                    new StandardFunctionary()
                    {
                        StandardId = standardId,
                        FunctionaryId = standardFunctionaryId,
                    });
            }
            await _dbContext.AddRangeAsync(standardFunctionaryToInsert, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        private IQueryable<StandardFunctionary> FindQuery()
        {
            return _dbContext.StandardFunctionaries.AsQueryable();
        }
    }
}
