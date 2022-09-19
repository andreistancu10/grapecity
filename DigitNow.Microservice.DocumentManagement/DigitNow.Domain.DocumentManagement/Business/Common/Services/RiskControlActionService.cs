using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Risks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IRiskControlActionService
    {
        Task AddRangeAsync(List<RiskControlActionDto> riskControlActions, Risk risk, CancellationToken cancellationToken);
        IQueryable<RiskControlAction> FindQuery();
    }
    public class RiskControlActionService : IRiskControlActionService
    {
        private readonly DocumentManagementDbContext _dbContext;

        public RiskControlActionService(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRangeAsync(List<RiskControlActionDto> riskControlActions, Risk risk, CancellationToken cancellationToken)
        {
            if (!riskControlActions.Any()) return;

            var riskControlActionsToAdd = new List<RiskControlAction>();

            riskControlActions.ForEach(controlledAction =>
            {
                if (controlledAction.Id == null)
                    riskControlActionsToAdd.Add(new RiskControlAction
                    {
                        RiskId = risk.Id,
                        ControlMeasurement = controlledAction.ControlMeasurement,
                        Deadline = controlledAction.Deadline,
                    });
            });

            if(riskControlActionsToAdd.Count == 0) return;  
            await _dbContext.AddRangeAsync(riskControlActionsToAdd, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<RiskControlAction> FindQuery()
        {
            return _dbContext.RiskControlActions.AsQueryable();
        }
    }
}
