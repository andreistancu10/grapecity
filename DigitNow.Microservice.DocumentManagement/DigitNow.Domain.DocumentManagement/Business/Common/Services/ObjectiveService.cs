using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using System.Text;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IObjectiveService
    {
        Task<Objective> AddAsync(Objective objective, CancellationToken token);
        Task<Objective> FindAsync(Expression<Func<Objective, bool>> predicate, CancellationToken token, params Expression<Func<Objective, object>>[] includes);
        Task<List<Objective>> FindAllAsync(Expression<Func<Objective, bool>> predicate, CancellationToken token);
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

        public Task<Objective> FindAsync(Expression<Func<Objective, bool>> predicate, CancellationToken token, params Expression<Func<Objective, object>>[] includes)
        {
            return _dbContext.Objectives
                .Includes(includes)
                .FirstOrDefaultAsync(predicate, token);
        }

        public Task<List<Objective>> FindAllAsync(Expression<Func<Objective, bool>> predicate, CancellationToken token)
        {
            return _dbContext.Objectives
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync(token);
        }

        private async Task SetGeneralObjectiveCodeAsync(Objective objective, CancellationToken token)
        {
            var maxObjectiveCode = await _dbContext.Objectives
                .Where(item => item.ObjectiveType == ObjectiveType.General && item.CreatedAt.Year == DateTime.UtcNow.Year)
                .CountAsync(token);

            objective.Code = $"OG{DateTime.Now.Year}_{++maxObjectiveCode}";
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
                var generalObjective = await FindAsync(item => item.Id == objective.SpecificObjective.GeneralObjectiveId, token);
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
