using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IGeneralObjectiveService : IScimStateService
    {
        Task<GeneralObjective> AddAsync(GeneralObjective generalObjective, CancellationToken cancellationToken);
        Task UpdateAsync(GeneralObjective generalObjective, CancellationToken cancellationToken);
        IQueryable<GeneralObjective> FindQuery();
    }

    public class GeneralObjectiveService : ScimStateService, IGeneralObjectiveService
    {
        private readonly IObjectiveService _objectiveService;

        public GeneralObjectiveService(
            DocumentManagementDbContext dbContext,
            IObjectiveService objectiveService) : base(dbContext)
        {
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
            await DbContext.AddAsync(generalObjective, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);

            return generalObjective;
        }

        public IQueryable<GeneralObjective> FindQuery()
        {
            return DbContext.GeneralObjectives.AsQueryable();
        }

        public async Task UpdateAsync(GeneralObjective generalObjective, CancellationToken cancellationToken)
        {
            await ChangeStateAsync(new List<long> { generalObjective.ObjectiveId }, ScimEntity.GeneralObjective, generalObjective.Objective?.State, cancellationToken);
            await DbContext.SingleUpdateAsync(generalObjective, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}