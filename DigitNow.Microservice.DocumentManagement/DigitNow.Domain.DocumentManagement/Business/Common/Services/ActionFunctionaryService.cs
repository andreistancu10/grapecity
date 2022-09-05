using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Actions;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IActionFunctionaryService
    {
        Task AddRangeAsync(long actionId, List<long> functionaryIds, CancellationToken cancellationToken);
        Task UpdateRangeAsync(long actionId, List<long> functionaryIds, CancellationToken cancellationToken);

    }
    public class ActionFunctionaryService : IActionFunctionaryService
    {
        private readonly DocumentManagementDbContext _dbContext;
        public ActionFunctionaryService(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddRangeAsync(long actionId, List<long> functionaryIds, CancellationToken cancellationToken)
        {
            if (!functionaryIds.Any()) return;

            var actionFunctionaryToInsert = new List<ActionFunctionary>();
            foreach (var functionaryId in functionaryIds)
            {
                actionFunctionaryToInsert.Add(
                    new ActionFunctionary()
                    {
                        ActionId = actionId,
                        FunctionaryId = functionaryId,
                    });
            }
            await _dbContext.AddRangeAsync(actionFunctionaryToInsert, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateRangeAsync(long actionId, List<long> functionaryIds, CancellationToken cancellationToken)
        {
            if (!functionaryIds.Any()) return;

            var initialActionFunctionary = await FindQuery().Where(item => item.ActionId == actionId).ToListAsync(cancellationToken);

            if (initialActionFunctionary.Count != 0)
            {
                var initialFunctionaryIds = initialActionFunctionary.Select(x => x.FunctionaryId).ToList();
                var functionaryIdsToRemove = initialFunctionaryIds.Except(functionaryIds);

                if (functionaryIdsToRemove.Any())
                {
                    var itemsToDelete = initialActionFunctionary.Where(x => functionaryIdsToRemove.Any(y => y == x.FunctionaryId)).ToArray();
                    _dbContext.ActionFunctionaries.RemoveRange(itemsToDelete);
                }
                functionaryIds = functionaryIds.Except(initialFunctionaryIds).ToList();
            }

            var actionFunctionaryToInsert = new List<ActionFunctionary>();
            foreach (var actionFunctionaryId in functionaryIds)
            {
                actionFunctionaryToInsert.Add(
                    new ActionFunctionary()
                    {
                        ActionId = actionId,
                        FunctionaryId = actionFunctionaryId,
                    });
            }
            await _dbContext.AddRangeAsync(actionFunctionaryToInsert, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public IQueryable<ActionFunctionary> FindQuery()
        {
            return _dbContext.ActionFunctionaries.AsQueryable();
        }
    }
}
