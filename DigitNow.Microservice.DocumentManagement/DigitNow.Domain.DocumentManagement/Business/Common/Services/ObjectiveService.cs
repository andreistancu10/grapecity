using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IObjectiveService
    {
        Task<Objective> AddAsync(Objective objective, CancellationToken token);
        IQueryable<Objective> FindQuery();
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
                _dbContext.Entry(objective).State = EntityState.Added;
                if (objective.ObjectiveType == ObjectiveType.General)
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
                await dbContextTransaction.RollbackAsync(token);
                throw;
            }
            finally
            {
                dbContextTransaction?.Dispose();
            }

            return objective;
        }

        public IQueryable<Objective> FindQuery()
        {
            return _dbContext.Objectives.AsQueryable();
        }

        private async Task SetGeneralObjectiveCodeAsync(Objective objective, CancellationToken token)
        {
            var lastObjective = await _dbContext.GeneralObjectives
                .Where(item => item.CreatedAt.Year == DateTime.UtcNow.Year)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync(token);

            var order = lastObjective != null ? lastObjective.Id : 0;

            objective.Code = $"OG{DateTime.Now.Year}_{++order}";
        }

        private async Task SetSpecificObjectiveCodeAsync(Objective objective, CancellationToken token)
        {
            var lastSpecificObjective = await _dbContext.SpecificObjectives
                    .Where(item => item.GeneralObjectiveId == objective.SpecificObjective.GeneralObjectiveId)
                    .Include(o => o.Objective)
                    .OrderByDescending(p => p.CreatedAt)
                    .FirstOrDefaultAsync(token);

            var sb = new StringBuilder();
            if (lastSpecificObjective == null)
            {
                var generalObjective = await FindQuery().Where(item => item.Id == objective.SpecificObjective.GeneralObjectiveId).FirstOrDefaultAsync(token);
                sb.Append(generalObjective.Code.Replace("OG", "OS"));
                sb.Append(".1");
            }
            else
            {
                string[] subs = lastSpecificObjective.Objective.Code.Split('.');
                sb.Append(subs[0]);
                sb.Append('.');
                sb.Append(int.Parse(subs[1]) + 1);
            }
            objective.Code = sb.ToString();
        }
    }
}
