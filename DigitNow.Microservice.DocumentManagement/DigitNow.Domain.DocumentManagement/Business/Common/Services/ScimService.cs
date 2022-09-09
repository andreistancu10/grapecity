using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
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

        public virtual async Task ChangeStateAsync(
            ICollection<long> entityIds, 
            ScimEntity scimEntity, 
            ScimState? state,
            CancellationToken cancellationToken)
        {
            if (!entityIds.Any() || state == null)
            {
                return;
            }

            switch (scimEntity)
            {
                case ScimEntity.GeneralObjective:
                    await ChangeGeneralObjectivesStateAsync(entityIds, (ScimState)state, cancellationToken);
                    break;

                case ScimEntity.SpecificObjective:
                    await ChangeSpecificObjectivesStateAsync(entityIds, (ScimState)state, cancellationToken);
                    break;

                case ScimEntity.ScimActivity:
                    await ChangeActivitiesStateAsync(entityIds, (ScimState)state, cancellationToken);
                    break;

                case ScimEntity.ScimAction:
                    await ChangeActionsStateAsync(entityIds, (ScimState)state, cancellationToken);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(scimEntity), scimEntity, null);
            }

            await DbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task ChangeGeneralObjectivesStateAsync(ICollection<long> entityIds, ScimState state, CancellationToken cancellationToken)
        {
            var generalObjectives = await DbContext.GeneralObjectives
                .Include(c => c.Objective)
                .Where(c => entityIds.Contains(c.Id) && c.Objective.State != state)
                .ToListAsync(cancellationToken);

            generalObjectives.ForEach(c => c.Objective.State = ScimState.Inactive);
            DbContext.GeneralObjectives.UpdateRange(generalObjectives);

            var specificObjectiveIds = await DbContext.SpecificObjectives
                .Where(c => entityIds.Contains(c.GeneralObjectiveId) && c.Objective.State == ScimState.Active)
                .Select(c => c.Id)
                .ToListAsync(cancellationToken);

            var generalObjectiveActivityIds = await DbContext.Activities
                .Where(c => entityIds.Contains(c.GeneralObjectiveId) && c.State != state)
                .Select(c => c.Id)
                .ToListAsync(cancellationToken);

            await ChangeStateAsync(specificObjectiveIds, ScimEntity.SpecificObjective, state, cancellationToken);
            await ChangeStateAsync(generalObjectiveActivityIds, ScimEntity.ScimActivity, state, cancellationToken);
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
                .Where(c => entityIds.Contains(c.SpecificObjectiveId) && c.State != state)
                .Select(c => c.Id)
                .ToListAsync(cancellationToken);

            await ChangeActivitiesStateAsync(activityIds, state, cancellationToken);
        }

        private async Task ChangeActivitiesStateAsync(ICollection<long> entityIds, ScimState state, CancellationToken cancellationToken)
        {
            var activities = await DbContext.Activities
                .Where(c => entityIds.Contains(c.Id) && c.State != state)
                .ToListAsync(cancellationToken);

            activities.ForEach(c => c.State = state);
            DbContext.Activities.UpdateRange(activities);

            var actionIds = await DbContext.Actions
                .Where(c => entityIds.Contains(c.ActivityId) && c.State != state)
                .Select(c => c.Id)
                .ToListAsync(cancellationToken);

            await ChangeActionsStateAsync(actionIds, state, cancellationToken);
        }

        private async Task ChangeActionsStateAsync(ICollection<long> entityIds, ScimState state, CancellationToken cancellationToken)
        {
            var actions = await DbContext.Actions
                .Where(c => entityIds.Contains(c.Id) && c.State != state)
                .ToListAsync(cancellationToken);

            actions.ForEach(c => c.State = state);
            DbContext.Actions.UpdateRange(actions);
        }
    }
}