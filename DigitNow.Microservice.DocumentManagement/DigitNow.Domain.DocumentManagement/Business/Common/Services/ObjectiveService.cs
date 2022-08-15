using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IObjectiveService
    {
        Task<Objective> AddAsync(Objective objective, CancellationToken token);
    }
    public class ObjectiveService : IObjectiveService
    {
        protected readonly DocumentManagementDbContext _dbContext;

        public ObjectiveService(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Objective> AddAsync(Objective objective, CancellationToken token)
        {
            var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, token);
            try
            {
                // Insert the entity without relationships
                _dbContext.Entry(objective).State = EntityState.Added;
                if(objective.ObjectiveType == ObjectiveType.General)
                {
                    await SetGeneralObjectiveCodeAsync(objective, token);
                }
                else
                {
                    await SetSpecificObjectiveCodeAsync(objective, token);
                }
               
                await _dbContext.SaveChangesAsync(token);
                await dbContextTransaction.CommitAsync(token);
            }
            catch
            {
                //TODO: Log error
                await dbContextTransaction.RollbackAsync(token);
                throw;
            }
            finally
            {
                dbContextTransaction?.Dispose();
            }

            return objective;
        }

        private async Task SetGeneralObjectiveCodeAsync(Objective objective, CancellationToken token)
        {
            var maxObjectiveCode = await _dbContext.GeneralObjectives
                .Select(x => x.Id)
                .MaxAsync(token);

            objective.Code = $"OG{DateTime.Now.Year}_{++maxObjectiveCode}";
        }

        private async Task SetSpecificObjectiveCodeAsync(Objective objective, CancellationToken token)
        {
            var maxObjectiveCode = await _dbContext.SpecificObjectives
                .Select(x => x.Id)
                .MaxAsync(token);

            objective.Code = $"OS{DateTime.Now.Year}_{++maxObjectiveCode}";
        }
    }
}
