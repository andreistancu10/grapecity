using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IGeneralObjectiveService
    {
        Task<GeneralObjective> AddAsync(GeneralObjective generalObjective, CancellationToken cancellationToken);
        Task UpdateAsync(GeneralObjective generalObjective, CancellationToken cancellationToken);
        IQueryable<GeneralObjective> FindQuery(Expression<Func<GeneralObjective, bool>> predicate, params Expression<Func<GeneralObjective, object>>[] includes);
        IQueryable<GeneralObjective> FindAllQuery(Expression<Func<GeneralObjective, bool>> predicate);
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
            generalObjective.Objective.State = ObjectiveState.Active;

            await _objectiveService.AddAsync(generalObjective.Objective, cancellationToken);
            await _dbContext.AddAsync(generalObjective, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return generalObjective;
        }

        public IQueryable<GeneralObjective> FindAllQuery(Expression<Func<GeneralObjective, bool>> predicate)
        {
            return _dbContext.GeneralObjectives
               .Include(x => x.Objective)
               .Where(predicate);
        }

        public IQueryable<GeneralObjective> FindQuery(Expression<Func<GeneralObjective, bool>> predicate, params Expression<Func<GeneralObjective, object>>[] includes)
        {
            return _dbContext.GeneralObjectives
               .Includes(includes)
               .Where(predicate);
        }

        public async Task UpdateAsync(GeneralObjective generalObjective, CancellationToken cancellationToken)
        {
            await _dbContext.SingleUpdateAsync(generalObjective, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

        }
    }
}