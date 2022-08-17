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
        Task<SpecificObjective> FindAsync(Expression<Func<SpecificObjective, bool>> predicate, CancellationToken token, params Expression<Func<SpecificObjective, object>>[] includes);
        Task<List<SpecificObjective>> FindAllAsync(Expression<Func<SpecificObjective, bool>> predicate, CancellationToken cancellationToken);
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

            if (specificObjective.GeneralObjectiveId != 0)
            {
                var generalObjective = await _generalObjectiveService.FindAsync(x => x.ObjectiveId == specificObjective.GeneralObjectiveId, cancellationToken);

                if (generalObjective.SpecificObjectives == null)
                    generalObjective.SpecificObjectives = new List<SpecificObjective>() { specificObjective };
                else
                    generalObjective.SpecificObjectives.Add(specificObjective);

                
                specificObjective.AssociatedGeneralObjective = generalObjective;
            }

            await _dbContext.SpecificObjectives.AddAsync(specificObjective, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return specificObjective;
        }

        public async Task<List<SpecificObjective>> FindAllAsync(Expression<Func<SpecificObjective, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbContext.SpecificObjectives
              .Include(x => x.Objective)
              .Where(predicate)
              .ToListAsync(cancellationToken);
        }

        public async Task<SpecificObjective> FindAsync(Expression<Func<SpecificObjective, bool>> predicate, CancellationToken token, params Expression<Func<SpecificObjective, object>>[] includes)
        {
            return await _dbContext.SpecificObjectives
               .Includes(includes)
               .FirstOrDefaultAsync(predicate, token);
        }

        public async Task UpdateAsync(SpecificObjective specificObjective, CancellationToken cancellationToken)
        {
            await _dbContext.SingleUpdateAsync(specificObjective, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
