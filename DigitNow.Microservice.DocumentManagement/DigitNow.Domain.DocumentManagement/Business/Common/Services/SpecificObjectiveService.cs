using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Contracts.Scim;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface ISpecificObjectiveService : IScimStateService
    {
        Task<SpecificObjective> AddAsync(SpecificObjective specificObjective, CancellationToken cancellationToken);
        Task UpdateAsync(SpecificObjective specificObjective, CancellationToken cancellationToken);
        IQueryable<SpecificObjective> FindQuery();
    }
    public class SpecificObjectiveService : ScimStateService, ISpecificObjectiveService
    {
        private readonly IObjectiveService _objectiveService;
        private readonly IGeneralObjectiveService _generalObjectiveService;

        public SpecificObjectiveService(
            DocumentManagementDbContext dbContext,
            IObjectiveService objectiveService,
            IGeneralObjectiveService generalObjectiveService) : base(dbContext)
        {
            _objectiveService = objectiveService;
            _generalObjectiveService = generalObjectiveService;
        }

        public async Task<SpecificObjective> AddAsync(SpecificObjective specificObjective, CancellationToken cancellationToken)
        {
            if (specificObjective.Objective == null)
            {
                specificObjective.Objective = new Objective();
            }

            specificObjective.Objective.ObjectiveType = ObjectiveType.Specific;
            specificObjective.Objective.State = ScimState.Active;
            specificObjective.Objective.SpecificObjective = specificObjective;
            await _objectiveService.AddAsync(specificObjective.Objective, cancellationToken);

            var generalObjective = await _generalObjectiveService.FindQuery().Where(x => x.ObjectiveId == specificObjective.GeneralObjectiveId).FirstOrDefaultAsync(cancellationToken);

            if (generalObjective.SpecificObjectives == null)
                generalObjective.SpecificObjectives = new List<SpecificObjective>() { specificObjective };
            else
                generalObjective.SpecificObjectives.Add(specificObjective);

            specificObjective.AssociatedGeneralObjective = generalObjective;

            await DbContext.SpecificObjectives.AddAsync(specificObjective, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);

            return specificObjective;
        }

        public IQueryable<SpecificObjective> FindAllQuery(Expression<Func<SpecificObjective, bool>> predicate)
        {
            return DbContext.SpecificObjectives
              .Include(x => x.Objective)
              .Where(predicate);
        }

        public IQueryable<SpecificObjective> FindQuery()
        {
            return DbContext.SpecificObjectives.AsQueryable();
        }

        public async Task UpdateAsync(SpecificObjective specificObjective, CancellationToken cancellationToken)
        {
            DbContext.Entry(specificObjective).State = EntityState.Modified;
            await ChangeStateAsync(new List<long> { specificObjective.ObjectiveId }, ScimEntity.SpecificObjective, specificObjective.Objective?.State, cancellationToken);
            await DbContext.SingleUpdateAsync(specificObjective, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
