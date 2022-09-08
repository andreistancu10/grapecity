using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;
using DigitNow.Domain.DocumentManagement.Business.Activities.Queries.FilterActivities;
using HTSS.Platform.Infrastructure.Data.Abstractions;
using HTSS.Platform.Infrastructure.Data.EntityFramework;
using Nest;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IActivityService
    {
        Task<PagedList<Activity>> GetActivitiesAsync(FilterActivitiesQuery request, CancellationToken cancellationToken);
        Task<long> CountActivitiesAsync(FilterActivitiesQuery query, CancellationToken cancellationToken);
        Task<Activity> AddAsync(Activity activity, CancellationToken cancellationToken);
        Task UpdateAsync(Activity activity, CancellationToken cancellationToken);
        IQueryable<Activity> FindQuery();
    }

    public class ActivityService : IActivityService
    {
        private readonly DocumentManagementDbContext _dbContext;

        public ActivityService(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<Activity>> GetActivitiesAsync(FilterActivitiesQuery request,
            CancellationToken cancellationToken)
        {
            var descriptor = new FilterDescriptor<Activity>(_dbContext.Activities.AsNoTracking(), request.SortField, request.SortOrder);
            var pagedResult = await descriptor.PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);

            return pagedResult;
        }

        public async Task<long> CountActivitiesAsync(FilterActivitiesQuery query,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Activities.CountAsync(c => query.DepartmentIds.Contains(c.DepartmentId), cancellationToken);
        }

        public async Task<Activity> AddAsync(Activity activity, CancellationToken token)
        {
            activity.State = ScimState.Active;
            var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, token);
            try
            {
                _dbContext.Entry(activity).State = EntityState.Added;
                await SetActivityCodeAsync(activity, token);
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

            return activity;
        }

        public IQueryable<Activity> FindQuery()
        {
            return _dbContext.Activities.AsQueryable();
        }

        public async Task UpdateAsync(Activity activity, CancellationToken cancellationToken)
        {
            await _dbContext.SingleUpdateAsync(activity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task SetActivityCodeAsync(Activity activity, CancellationToken token)
        {
            var lastActivity = await _dbContext.Activities
                    .Where(item => item.SpecificObjectiveId == activity.SpecificObjectiveId)
                    .OrderByDescending(p => p.CreatedAt)
                    .FirstOrDefaultAsync(token);

            var sb = new StringBuilder();
            if (lastActivity == null)
            {
                var specificObjective = await _dbContext.SpecificObjectives
                    .Where(item => item.ObjectiveId == activity.SpecificObjectiveId)
                    .Include(o => o.Objective)
                    .FirstOrDefaultAsync(token);

                sb.Append(specificObjective.Objective.Code.Replace("OS", "ACTIV"));
                sb.Append(".1");
            }
            else
            {
                string[] subs = lastActivity.Code.Split('.');
                sb.Append(subs[0]);
                sb.Append('.');
                sb.Append(subs[1]);
                sb.Append('.');
                sb.Append(int.Parse(subs[2]) + 1);
            }
            activity.Code = sb.ToString();
        }
    }
}
