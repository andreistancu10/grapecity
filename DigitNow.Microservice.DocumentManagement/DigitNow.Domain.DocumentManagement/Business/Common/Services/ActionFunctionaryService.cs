using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Actions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IActionFunctionaryService
    {
        Task AddRangeAsync(long actionId, List<long> functionaryIds, CancellationToken cancellationToken);
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
    }
}
