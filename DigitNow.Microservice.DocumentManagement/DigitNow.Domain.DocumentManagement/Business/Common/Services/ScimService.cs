using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Contracts.Scim;
using DigitNow.Domain.DocumentManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IScimStateService
    {
        Task ChangeStateAsync(ICollection<long> entityIds, ScimEntity scimEntity, ScimState? state,
            CancellationToken cancellationToken);
    }

    public abstract class ScimStateService : IScimStateService
    {
        protected readonly DocumentManagementDbContext DbContext;

        protected ScimStateService(DocumentManagementDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual Task ChangeStateAsync(
            ICollection<long> entityIds,
            ScimEntity scimEntity,
            ScimState? state,
            CancellationToken cancellationToken)
        {
            if (!entityIds.Any() || !state.HasValue)
            {
                return Task.CompletedTask;
            }

            return scimEntity switch
            {
                ScimEntity.GeneralObjective => ChangeGeneralObjectivesStateAsync(entityIds, state.Value, cancellationToken),
                ScimEntity.SpecificObjective => ChangeSpecificObjectivesStateAsync(entityIds, state.Value, cancellationToken),
                ScimEntity.ScimActivity => ChangeActivitiesStateAsync(entityIds, state.Value, cancellationToken),
                ScimEntity.ScimAction => ChangeActionsStateAsync(entityIds, state.Value, cancellationToken),
                ScimEntity.ScimRisk => ChangeRisksStateAsync(entityIds, state.Value, cancellationToken),
                ScimEntity.ScimProcedure => ChangeProceduresStateAsync(entityIds, state.Value, cancellationToken),
                _ => throw new ArgumentOutOfRangeException(nameof(scimEntity), scimEntity, null),
            };
        }

        private async Task ChangeGeneralObjectivesStateAsync(ICollection<long> entityIds, ScimState state, CancellationToken cancellationToken)
        {
            var generalObjectives = await DbContext.GeneralObjectives
                .Include(c => c.Objective)
                .Where(c => entityIds.Contains(c.Id) && c.Objective.State != state)
                .ToListAsync(cancellationToken);

            generalObjectives.ForEach(c => c.Objective.State = state);
            DbContext.GeneralObjectives.UpdateRange(generalObjectives);

            var specificObjectiveIds = await DbContext.SpecificObjectives
                .Where(c => entityIds.Contains(c.GeneralObjectiveId))
                .Select(c => c.Id)
                .ToListAsync(cancellationToken);

            await ChangeStateAsync(specificObjectiveIds, ScimEntity.SpecificObjective, state, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task ChangeSpecificObjectivesStateAsync(ICollection<long> entityIds, ScimState state, CancellationToken cancellationToken)
        {
            var specificObjectives = await DbContext.SpecificObjectives
                .Include(c => c.Objective)
                .Where(c => entityIds.Contains(c.Id) && c.Objective.State != state)
                .ToListAsync(cancellationToken);

            specificObjectives.ForEach(c => c.Objective.State = state);
            DbContext.SpecificObjectives.UpdateRange(specificObjectives);

            var activityIds = await DbContext.Activities
                .Where(c => entityIds.Contains(c.SpecificObjectiveId))
                .Select(c => c.Id)
                .ToListAsync(cancellationToken);

            await ChangeStateAsync(activityIds, ScimEntity.ScimActivity, state, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task ChangeActivitiesStateAsync(ICollection<long> entityIds, ScimState state, CancellationToken cancellationToken)
        {
            var activities = await DbContext.Activities
                .Where(c => entityIds.Contains(c.Id) && c.State != state)
                .ToListAsync(cancellationToken);

            activities.ForEach(c => c.State = state);
            DbContext.Activities.UpdateRange(activities);

            var actionIds = await DbContext.Actions
                .Where(c => entityIds.Contains(c.ActivityId))
                .Select(c => c.Id)
                .ToListAsync(cancellationToken);

            var riskIds = await DbContext.Risks
                .Where(c => entityIds.Contains(c.ActivityId))
                .Select(c => c.Id)
                .ToListAsync(cancellationToken);

            var procedureIds = await DbContext.Procedures
                .Where(c => entityIds.Contains(c.ActivityId))
                .Select(c => c.Id)
                .ToListAsync(cancellationToken);

            //TODO: add logic for indicator

            await ChangeStateAsync(actionIds, ScimEntity.ScimAction, state, cancellationToken);
            await ChangeStateAsync(riskIds, ScimEntity.ScimRisk, state, cancellationToken);
            await ChangeStateAsync(procedureIds, ScimEntity.ScimProcedure, state, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task ChangeActionsStateAsync(ICollection<long> entityIds, ScimState state, CancellationToken cancellationToken)
        {
            var actions = await DbContext.Actions
                .Where(c => entityIds.Contains(c.Id) && c.State != state)
                .ToListAsync(cancellationToken);

            actions.ForEach(c => c.State = state);
            DbContext.Actions.UpdateRange(actions);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task ChangeRisksStateAsync(ICollection<long> entityIds, ScimState state, CancellationToken cancellationToken)
        {
            var risks = await DbContext.Risks
                .Where(c => entityIds.Contains(c.Id) && c.State != state)
                .ToListAsync(cancellationToken);

            risks.ForEach(c => c.State = state);
            DbContext.Risks.UpdateRange(risks);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task ChangeProceduresStateAsync(ICollection<long> entityIds, ScimState state, CancellationToken cancellationToken)
        {
            var procedures = await DbContext.Procedures
                .Where(c => entityIds.Contains(c.Id) && c.State != state)
                .ToListAsync(cancellationToken);

            procedures.ForEach(c => c.State = state);
            DbContext.Procedures.UpdateRange(procedures);
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}