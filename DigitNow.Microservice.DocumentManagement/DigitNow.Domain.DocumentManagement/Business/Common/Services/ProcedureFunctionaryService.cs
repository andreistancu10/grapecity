using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Procedures;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IProcedureFunctionaryService
    {
        Task AddRangeAsync(long id, List<long> functionaryIds, CancellationToken cancellationToken);
        Task UpdateRangeAsync(long id, List<long> functionaryIds, CancellationToken cancellationToken);
        IQueryable<ProcedureFunctionary> FindQuery();
    }
    public class ProcedureFunctionaryService : IProcedureFunctionaryService
    {
        private readonly DocumentManagementDbContext _dbContext;

        public ProcedureFunctionaryService(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddRangeAsync(long id, List<long> functionaryIds, CancellationToken cancellationToken)
        {
            if (!functionaryIds.Any()) return;

            var procedureFunctionaryToInsert = new List<ProcedureFunctionary>();
            foreach (var functionaryId in functionaryIds)
            {
                procedureFunctionaryToInsert.Add(
                    new ProcedureFunctionary()
                    {
                        ProcedureId = id,
                        FunctionaryId = functionaryId,
                    });
            }
            await _dbContext.AddRangeAsync(procedureFunctionaryToInsert, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<ProcedureFunctionary> FindQuery()
        {
            return _dbContext.ProcedureFunctionarys.AsQueryable();
        }

        public async Task UpdateRangeAsync(long id, List<long> functionaryIds, CancellationToken cancellationToken)
        {
            if (!functionaryIds.Any()) return;

            var initialProcedureFunctionary = await FindQuery().Where(item => item.ProcedureId == id).ToListAsync(cancellationToken);

            if (initialProcedureFunctionary.Count != 0)
            {
                var initialFunctionaryIds = initialProcedureFunctionary.Select(x => x.FunctionaryId).ToList();
                var functionaryIdsToRemove = initialFunctionaryIds.Except(functionaryIds);

                if (functionaryIdsToRemove.Any())
                {
                    var itemsToDelete = initialProcedureFunctionary.Where(x => functionaryIdsToRemove.Any(y => y == x.FunctionaryId)).ToArray();
                    _dbContext.ProcedureFunctionarys.RemoveRange(itemsToDelete);
                }
                functionaryIds = functionaryIds.Except(initialFunctionaryIds).ToList();
            }

            var procedureFunctionaryToInsert = new List<ProcedureFunctionary>();
            foreach (var procedureFunctionaryId in functionaryIds)
            {
                procedureFunctionaryToInsert.Add(
                    new ProcedureFunctionary()
                    {
                        ProcedureId = id,
                        FunctionaryId = procedureFunctionaryId,
                    });
            }
            await _dbContext.AddRangeAsync(procedureFunctionaryToInsert, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
