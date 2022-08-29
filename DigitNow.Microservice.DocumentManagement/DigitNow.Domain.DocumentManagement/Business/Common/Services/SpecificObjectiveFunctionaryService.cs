using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface ISpecificObjectiveFunctionaryService
    {
        Task AddRangeAsync(long objectiveId, List<long> functionaryIds, CancellationToken cancellationToken);
        Task UpdateRangeAsync(long objectiveId, List<long> functionaryIds, CancellationToken cancellationToken);
        IQueryable<SpecificObjectiveFunctionary> FindQuery();
    }
    public class SpecificObjectiveFunctionaryService : ISpecificObjectiveFunctionaryService
    {
        private readonly DocumentManagementDbContext _dbContext;

        public SpecificObjectiveFunctionaryService(DocumentManagementDbContext dbContext, ISpecificObjectiveService specificObjectiveService)
        {
            _dbContext = dbContext;
        }
        public async Task AddRangeAsync(long objectiveId, List<long> functionaryIds, CancellationToken cancellationToken)
        {
            if (!functionaryIds.Any()) return;

            var specificObjectiveFunctionaryToInsert = new List<SpecificObjectiveFunctionary>();
            foreach (var functionaryId in functionaryIds)
            {
                specificObjectiveFunctionaryToInsert.Add(
                    new SpecificObjectiveFunctionary()
                    {
                        SpecificObjectiveId = objectiveId,
                        FunctionaryId = functionaryId,
                    });
            }
            await _dbContext.AddRangeAsync(specificObjectiveFunctionaryToInsert, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<SpecificObjectiveFunctionary> FindQuery()
        {
            return _dbContext.SpecificObjectiveFunctionaries.AsQueryable();
        }

        public async Task UpdateRangeAsync(long objectiveId, List<long> functionaryIds, CancellationToken cancellationToken)
        {
            if (!functionaryIds.Any()) return;

            var initialSpecificObjectiveFunctionary = await FindQuery().Where(item => item.SpecificObjectiveId == objectiveId).ToListAsync(cancellationToken);

            if (initialSpecificObjectiveFunctionary.Count != 0)
            {
                var initialFunctionaryIds = initialSpecificObjectiveFunctionary.Select(x => x.FunctionaryId).ToList();
                var functionaryIdsToRemove = initialFunctionaryIds.Except(functionaryIds);

                if (functionaryIdsToRemove.Any())
                {
                    var itemsToDelete = initialSpecificObjectiveFunctionary.Where(x => functionaryIdsToRemove.Any(y => y == x.FunctionaryId)).ToArray();
                    _dbContext.SpecificObjectiveFunctionaries.RemoveRange(itemsToDelete);
                }
                functionaryIds = functionaryIds.Except(initialFunctionaryIds).ToList();
            }

            var specificObjectiveFunctionaryToInsert = new List<SpecificObjectiveFunctionary>();
            foreach (var specificObjectiveFunctionaryId in functionaryIds)
            {
                specificObjectiveFunctionaryToInsert.Add(
                    new SpecificObjectiveFunctionary()
                    {
                        SpecificObjectiveId = objectiveId,
                        FunctionaryId = specificObjectiveFunctionaryId,
                    });
            }
            await _dbContext.AddRangeAsync(specificObjectiveFunctionaryToInsert, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
