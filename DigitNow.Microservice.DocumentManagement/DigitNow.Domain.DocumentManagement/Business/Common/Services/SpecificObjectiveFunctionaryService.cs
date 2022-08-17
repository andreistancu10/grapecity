using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface ISpecificObjectiveFunctionaryService
    {
        Task AddRangeAsync(List<long> functionaryIds, SpecificObjective specificObjective, CancellationToken cancellationToken);
        Task UpdateRangeAsync(List<long> functionaryIds, SpecificObjective specificObjective, CancellationToken cancellationToken);
        Task<List<SpecificObjectiveFunctionary>> FindAllAsyncByObjectiveId(Expression<Func<SpecificObjectiveFunctionary, bool>> predicate, CancellationToken cancellationToken);
    }
    public class SpecificObjectiveFunctionaryService : ISpecificObjectiveFunctionaryService
    {
        private readonly DocumentManagementDbContext _dbContext;

        public SpecificObjectiveFunctionaryService(DocumentManagementDbContext dbContext, ISpecificObjectiveService specificObjectiveService)
        {
            _dbContext = dbContext;
        }
        public async Task AddRangeAsync(List<long> functionaryIds, SpecificObjective specificObjective, CancellationToken cancellationToken)
        {
            if (!functionaryIds.Any()) return;

            var specificObjectiveFunctionaryToInsert = new List<SpecificObjectiveFunctionary>();
            foreach (var functionaryId in functionaryIds)
            {
                specificObjectiveFunctionaryToInsert.Add(
                    new SpecificObjectiveFunctionary()
                    {
                        SpecificObjectiveId = specificObjective.ObjectiveId,
                        FunctionaryId = functionaryId,
                        SpecificObjective = specificObjective
                    });
            }
            await _dbContext.AddRangeAsync(specificObjectiveFunctionaryToInsert, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<SpecificObjectiveFunctionary>> FindAllAsyncByObjectiveId(Expression<Func<SpecificObjectiveFunctionary, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbContext.SpecificObjectiveFunctionarys
               .Where(predicate)
               .ToListAsync(cancellationToken);
        }

        public async Task UpdateRangeAsync(List<long> functionaryIds, SpecificObjective specificObjective, CancellationToken cancellationToken)
        {
            if (!functionaryIds.Any()) return;

            var initialSpecificObjectiveFunctionary = await FindAllAsyncByObjectiveId(item => item.SpecificObjectiveId == specificObjective.ObjectiveId, cancellationToken);

            if (initialSpecificObjectiveFunctionary != null && initialSpecificObjectiveFunctionary.Count != 0)
            {
                var initialFunctionaryIds = initialSpecificObjectiveFunctionary.Select(x => x.FunctionaryId).ToList();
                var functionaryIdsToRemove = initialFunctionaryIds.Except(functionaryIds);

                if (functionaryIdsToRemove.Any())
                {
                    var itemsToDelete = initialSpecificObjectiveFunctionary.Where(x => functionaryIdsToRemove.Any(y => y == x.FunctionaryId)).ToArray();
                    _dbContext.SpecificObjectiveFunctionarys.RemoveRange(itemsToDelete);
                }
                functionaryIds = functionaryIds.Except(initialFunctionaryIds).ToList(); 
            }

            var specificObjectiveFunctionaryToInsert = new List<SpecificObjectiveFunctionary>();
            foreach (var specificObjectiveFunctionaryId in functionaryIds)
            {
                specificObjectiveFunctionaryToInsert.Add(
                    new SpecificObjectiveFunctionary()
                    {
                        SpecificObjectiveId = specificObjective.ObjectiveId,
                        FunctionaryId = specificObjectiveFunctionaryId,
                        SpecificObjective = specificObjective
                    });
            }
            await _dbContext.AddRangeAsync(specificObjectiveFunctionaryToInsert, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
