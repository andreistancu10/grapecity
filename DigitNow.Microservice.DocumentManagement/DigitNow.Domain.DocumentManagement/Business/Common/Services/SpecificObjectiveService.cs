using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface ISpecificObjectiveService
    {
        Task<SpecificObjective> AddAsync(SpecificObjective specificObjective, CancellationToken cancellationToken);
        Task UpdateAsync(SpecificObjective specificObjective, CancellationToken cancellationToken);
        IQueryable<SpecificObjective> FindQuery();
    }
    public class SpecificObjectiveService : ISpecificObjectiveService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IObjectiveService _objectiveService;
        private readonly IGeneralObjectiveService _generalObjectiveService;


        public SpecificObjectiveService(DocumentManagementDbContext dbContext,
            IObjectiveService objectiveService,
            IGeneralObjectiveService generalObjectiveService)
        {
            _dbContext = dbContext;
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
            specificObjective.Objective.State = ObjectiveState.Active;
            specificObjective.Objective.SpecificObjective = specificObjective;
            await _objectiveService.AddAsync(specificObjective.Objective, cancellationToken);

            var generalObjective = await _generalObjectiveService.FindQuery().Where(x => x.ObjectiveId == specificObjective.GeneralObjectiveId).FirstOrDefaultAsync(cancellationToken);

            if (generalObjective.SpecificObjectives == null)
                generalObjective.SpecificObjectives = new List<SpecificObjective>() { specificObjective };
            else
                generalObjective.SpecificObjectives.Add(specificObjective);

            specificObjective.AssociatedGeneralObjective = generalObjective;

            await _dbContext.SpecificObjectives.AddAsync(specificObjective, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return specificObjective;
        }

        public IQueryable<SpecificObjective> FindAllQuery(Expression<Func<SpecificObjective, bool>> predicate)
        {
            return _dbContext.SpecificObjectives
              .Include(x => x.Objective)
              .Where(predicate);
        }

        public IQueryable<SpecificObjective> FindQuery()
        {
            return _dbContext.SpecificObjectives.AsQueryable();
        }

        public async Task UpdateAsync(SpecificObjective specificObjective, CancellationToken cancellationToken)
        {
            await _dbContext.SingleUpdateAsync(specificObjective, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
