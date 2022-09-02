using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Data;
using Microsoft.Azure.Cosmos.Linq;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IActionService
    {
        Task<Data.Entities.Actions.Action> CreateAsync(Data.Entities.Actions.Action action, CancellationToken cancellationToken);

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
            await _dbContext.Actions.AddAsync(action, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return action;
        }
    }
}
