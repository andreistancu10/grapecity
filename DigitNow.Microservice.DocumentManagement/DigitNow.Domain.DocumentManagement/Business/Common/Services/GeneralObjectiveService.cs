using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IGeneralObjectiveService
    {
        Task<GeneralObjective> AddAsync(GeneralObjective generalObjective, CancellationToken cancellationToken);
        Task<List<GeneralObjective>> FindAllAsync(Expression<Func<GeneralObjective, bool>> predicate, CancellationToken cancellationToken);
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

        public Task<List<GeneralObjective>> FindAllAsync(Expression<Func<GeneralObjective, bool>> predicate, CancellationToken cancellationToken)
        {
            return _dbContext.GeneralObjectives
               .Include(x => x.Objective)
               .Where(predicate)
               .ToListAsync(cancellationToken);
        }
    }
}
