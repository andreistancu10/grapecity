using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IGeneralObjectiveService
    {
        Task<GeneralObjective> AddAsync(GeneralObjective generalObjective, CancellationToken cancellationToken);
        Task UpdateAsync(GeneralObjective generalObjective, CancellationToken cancellationToken);
        IQueryable<GeneralObjective> FindQuery();
    }
    public class GeneralObjectiveService : IGeneralObjectiveService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IObjectiveService _objectiveService;

        public GeneralObjectiveService(DocumentManagementDbContext dbContext, IObjectiveService objectiveService)
        {
            _dbContext = dbContext;
            _objectiveService = objectiveService;
        }

        public async Task<GeneralObjective> AddAsync(GeneralObjective generalObjective, CancellationToken cancellationToken)
        {
            if (generalObjective.Objective == null)
            {
                generalObjective.Objective = new Objective();
            }

            generalObjective.Objective.ObjectiveType = ObjectiveType.General;
            generalObjective.Objective.State = ScimState.Active;

            await _objectiveService.AddAsync(generalObjective.Objective, cancellationToken);
            await _dbContext.AddAsync(generalObjective, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return generalObjective;
        }

        public IQueryable<GeneralObjective> FindQuery()
        {
            return _dbContext.GeneralObjectives.AsQueryable();
        }

        public async Task UpdateAsync(GeneralObjective generalObjective, CancellationToken cancellationToken)
        {
            await _dbContext.SingleUpdateAsync(generalObjective, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

        }
    }
}