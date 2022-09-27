using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Contracts.Scim;
using DigitNow.Domain.DocumentManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IScimStateService
    {
        Task ChangeStateAsync(ICollection<long> entityIds, ScimEntity scimEntity, long? stateId,
            CancellationToken cancellationToken);
    }

    public abstract class ScimStateService : IScimStateService
    {
        protected readonly DocumentManagementDbContext DbContext;

        protected ScimStateService(DocumentManagementDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual async Task ChangeStateAsync(
            ICollection<long> entityIds,
            ScimEntity scimEntity,
            long? stateId,
            CancellationToken cancellationToken)
        {
            if (!entityIds.Any() || !stateId.HasValue)
            {
                return;
            }

            switch (scimEntity)
            {
                case ScimEntity.GeneralObjective:
                    await ChangeGeneralObjectivesStateAsync(entityIds, stateId.Value, cancellationToken);
                    break;

                case ScimEntity.SpecificObjective:
                    await ChangeSpecificObjectivesStateAsync(entityIds, stateId.Value, cancellationToken);
                    break;

                case ScimEntity.ScimActivity:
                    await ChangeActivitiesStateAsync(entityIds, stateId.Value, cancellationToken);
                    break;

                case ScimEntity.ScimAction:
                    await ChangeActionsStateAsync(entityIds, stateId.Value, cancellationToken);
                    break;

                case ScimEntity.ScimRisk:
                    await ChangeRisksStateAsync(entityIds, stateId.Value, cancellationToken);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(scimEntity), scimEntity, null);
            }
        }

        private async Task ChangeGeneralObjectivesStateAsync(ICollection<long> entityIds, long stateId, CancellationToken cancellationToken)
        {
            var generalObjectives = await DbContext.GeneralObjectives
                .Include(c => c.Objective)
                .Where(c => entityIds.Contains(c.ObjectiveId) && c.Objective.StateId != stateId)
                .ToListAsync(cancellationToken);

            generalObjectives.ForEach(c => c.Objective.StateId = stateId);
            DbContext.GeneralObjectives.UpdateRange(generalObjectives);
            var generalObjectiveIds = generalObjectives.Select(c => c.Id);

            var specificObjectiveObjectiveIds = await DbContext.SpecificObjectives
                .Where(c => generalObjectiveIds.Contains(c.GeneralObjectiveId))
                .Select(c => c.ObjectiveId)
                .ToListAsync(cancellationToken);

            var generalObjectiveActivityIds = await DbContext.Activities
                .Where(c => generalObjectiveIds.Contains(c.GeneralObjectiveId))
                .Select(c => c.Id)
                .ToListAsync(cancellationToken);

            await ChangeStateAsync(specificObjectiveObjectiveIds, ScimEntity.SpecificObjective, stateId, cancellationToken);
            await ChangeStateAsync(generalObjectiveActivityIds, ScimEntity.ScimActivity, stateId, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task ChangeSpecificObjectivesStateAsync(ICollection<long> entityIds, long stateId, CancellationToken cancellationToken)
        {
            var specificObjectives = await DbContext.SpecificObjectives
                .Include(c => c.Objective)
                .Where(c => entityIds.Contains(c.ObjectiveId) && c.Objective.StateId != stateId)
                .ToListAsync(cancellationToken);

            specificObjectives.ForEach(c => c.Objective.StateId = stateId);
            DbContext.SpecificObjectives.UpdateRange(specificObjectives);
            var specificObjectiveIds = specificObjectives.Select(c => c.Id);

            var activityIds = await DbContext.Activities
                .Where(c => specificObjectiveIds.Contains(c.SpecificObjectiveId))
                .Select(c => c.Id)
                .ToListAsync(cancellationToken);

            await ChangeStateAsync(activityIds, ScimEntity.ScimActivity, stateId, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task ChangeActivitiesStateAsync(ICollection<long> entityIds, long stateId, CancellationToken cancellationToken)
        {
            var activities = await DbContext.Activities
                .Where(c => entityIds.Contains(c.Id) && c.StateId != stateId)
                .ToListAsync(cancellationToken);

            activities.ForEach(c => c.StateId = stateId);
            DbContext.Activities.UpdateRange(activities);

            var actionIds = await DbContext.Actions
                .Where(c => entityIds.Contains(c.ActivityId))
                .Select(c => c.Id)
                .ToListAsync(cancellationToken);

            await ChangeStateAsync(actionIds, ScimEntity.ScimAction, stateId, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task ChangeActionsStateAsync(ICollection<long> entityIds, long stateId, CancellationToken cancellationToken)
        {
            var actions = await DbContext.Actions
                .Where(c => entityIds.Contains(c.Id) && c.StateId != stateId)
                .ToListAsync(cancellationToken);

            actions.ForEach(c => c.StateId = stateId);
            DbContext.Actions.UpdateRange(actions);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task ChangeRisksStateAsync(ICollection<long> entityIds, long stateId, CancellationToken cancellationToken)
        {
            var risks = await DbContext.Risks
                .Where(c => entityIds.Contains(c.Id) && c.StateId != stateId)
                .ToListAsync(cancellationToken);

            risks.ForEach(c => c.StateId = stateId);
            DbContext.Risks.UpdateRange(risks);
        }
    }
}