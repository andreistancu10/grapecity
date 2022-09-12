using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;
using DigitNow.Domain.DocumentManagement.Business.Activities.Queries.FilterActivities;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Activities;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using HTSS.Platform.Infrastructure.Data.Abstractions;
using HTSS.Platform.Infrastructure.Data.EntityFramework;
using Nest;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IActivityService : IScimStateService
    {
        Task<PagedList<Activity>> GetActivitiesAsync(ActivityFilter filter, CancellationToken cancellationToken);
        Task<long> CountActivitiesAsync(ActivityFilter filter, CancellationToken cancellationToken);
        Task<Activity> AddAsync(Activity activity, CancellationToken cancellationToken);
        Task UpdateAsync(Activity activity, CancellationToken cancellationToken);
        IQueryable<Activity> FindQuery();
    }

    public class ActivityService : ScimStateService, IActivityService
    {
        public ActivityService(DocumentManagementDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PagedList<Activity>> GetActivitiesAsync(ActivityFilter filter,
            CancellationToken cancellationToken)
        {
            var descriptor = new FilterDescriptor<Activity>(DbContext.Activities.AsNoTracking(), filter.SortField, filter.SortOrder);
            var pagedResult = await descriptor.PaginateAsync(filter.PageNumber, filter.PageSize, cancellationToken);

            return pagedResult;
        }

        public async Task<long> CountActivitiesAsync(ActivityFilter filter,
            CancellationToken cancellationToken)
        {
            return await DbContext.Activities.CountAsync(c => filter.DepartmentIds.Contains(c.DepartmentId), cancellationToken);
        }

        public async Task<Activity> AddAsync(Activity activity, CancellationToken token)
        {
            activity.State = ScimState.Active;
            var dbContextTransaction = await DbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, token);
            try
            {
                DbContext.Entry(activity).State = EntityState.Added;
                await SetActivityCodeAsync(activity, token);
                await DbContext.SaveChangesAsync(token);
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
            return DbContext.Activities.AsQueryable();
        }

        public async Task UpdateAsync(Activity activity, CancellationToken cancellationToken)
        {
            await ChangeStateAsync(new List<long> { activity.Id }, ScimEntity.ScimActivity, activity.State, cancellationToken);
            await DbContext.SingleUpdateAsync(activity, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task SetActivityCodeAsync(Activity activity, CancellationToken token)
        {
            var lastActivity = await DbContext.Activities
                    .Where(item => item.SpecificObjectiveId == activity.SpecificObjectiveId)
                    .OrderByDescending(p => p.CreatedAt)
                    .FirstOrDefaultAsync(token);

            var sb = new StringBuilder();
            if (lastActivity == null)
            {
                var specificObjective = await DbContext.SpecificObjectives
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
