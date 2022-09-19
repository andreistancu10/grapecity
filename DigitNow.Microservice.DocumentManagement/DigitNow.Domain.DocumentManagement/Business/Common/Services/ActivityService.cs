using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Contracts.Scim;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Abstractions;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Activities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IActivityService : IScimStateService
    {
        Task<List<Activity>> GetActivitiesAsync(ActivityFilter filter, UserModel currentUser,
            int page, int count,
            CancellationToken cancellationToken);
        Task<long> CountActivitiesAsync(ActivityFilter filter, UserModel currentUser,
            CancellationToken cancellationToken);
        Task<Activity> AddAsync(Activity activity, CancellationToken cancellationToken);
        Task UpdateAsync(Activity activity, CancellationToken cancellationToken);
        IQueryable<Activity> FindQuery();
    }

    public class ActivityService : ScimStateService, IActivityService
    {
        private readonly IServiceProvider _serviceProvider;

        public ActivityService(
            DocumentManagementDbContext dbContext,
            IServiceProvider serviceProvider) : base(dbContext)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<List<Activity>> GetActivitiesAsync(ActivityFilter filter, UserModel currentUser, int page, int count, CancellationToken cancellationToken)
        {
            return await GetBuiltInActivitiesQuery()
                  .WhereAll((await GetActivitiesDataExpressions(filter, currentUser, cancellationToken)).ToPredicates())
                  .OrderByDescending(x => x.CreatedAt)
                  .Skip((page - 1) * count)
                  .Take(count)
                  .AsNoTracking()
                  .ToListAsync(cancellationToken);
        }

        public async Task<long> CountActivitiesAsync(ActivityFilter filter, UserModel currentUser, CancellationToken cancellationToken)
        {
            return await GetBuiltInActivitiesQuery()
                .WhereAll((await GetActivitiesDataExpressions(filter, currentUser, cancellationToken)).ToPredicates())
                .AsNoTracking()
                .CountAsync(cancellationToken);
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


        private async Task<DataExpressions<Activity>> GetActivitiesDataExpressions(
            ActivityFilter filter,
            UserModel currentUser,
            CancellationToken token)
        {
            var activitiesFilterComponent = new ActivitiesFilterComponent(_serviceProvider);
            var activitiesFilterComponentContext = new ActivitiesFilterComponentContext
            {
                CurrentUser = currentUser,
                ActivityFilter = filter
            };

            return await activitiesFilterComponent.ExtractDataExpressionsAsync(activitiesFilterComponentContext, token);
        }

        private IQueryable<Activity> GetBuiltInActivitiesQuery()
        {
            return DbContext.Activities
                .Include(c => c.ActivityFunctionarys)
                .Include(x => x.AssociatedGeneralObjective);
        }
    }

    internal class ActivitiesFilterComponentContext : IDataExpressionFilterComponentContext
    {
        public UserModel CurrentUser { get; set; }
        public ActivityFilter ActivityFilter { get; set; }
    }

    internal class ActivitiesFilterComponent : DataExpressionFilterComponent<Activity, ActivitiesFilterComponentContext>
    {
        public ActivitiesFilterComponent(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Task<DataExpressions<Activity>> SetCustomDataExpressionsAsync(ActivitiesFilterComponentContext context, CancellationToken token)
        {
            var currentUser = context.CurrentUser;

            var filter = new ActivitiesPermissionsFilter
            {
                UserPermissionsFilter = new ActivityUserPermissionsFilter
                {
                    DepartmentIds = SetDepartmentsFilter(context.ActivityFilter.DepartmentsFilter.DepartmentIds, currentUser)
                },
                ActivitiesFilter = new ActivityActivitiesFilter
                {
                    ActivityIds = context.ActivityFilter.ActivitiesFilter.ActivityIds,
                },
                FunctionariesFilter = new ActivityFunctionariesFilter
                {
                    FunctionaryIds = context.ActivityFilter.FunctionariesFilter.FunctionaryIds
                },
                SpecificObjectivesFilter = new ActivitySpecificObjectivesFilter
                {
                    SpecificObjectiveIds = context.ActivityFilter.SpecificObjectivesFilter.SpecificObjectiveIds
                }
            };

            return Task.FromResult(new ActivityFilterBuilder(ServiceProvider, filter)
                .Build());
        }

        private List<long> SetDepartmentsFilter(IReadOnlyCollection<long> departmentsFilterDepartmentIds, UserModel currentUser)
        {
            departmentsFilterDepartmentIds=departmentsFilterDepartmentIds.Any()
                ? currentUser.Departments.Select(c => c.Id).Intersect(departmentsFilterDepartmentIds).ToList()
                : currentUser.Departments.Select(c => c.Id).ToList();

            if (!departmentsFilterDepartmentIds.Any())
            {
                departmentsFilterDepartmentIds = currentUser.Departments.Select(c => c.Id).ToList();
            }

            return departmentsFilterDepartmentIds.ToList();
        }
    }

    internal class ActivitiesPermissionsFilter : DataFilter
    {
        public ActivitySpecificObjectivesFilter SpecificObjectivesFilter { get; set; }
        public ActivityActivitiesFilter ActivitiesFilter { get; set; }
        public ActivityFunctionariesFilter FunctionariesFilter { get; set; }
        public ActivityUserPermissionsFilter UserPermissionsFilter { get; set; }
    }

    internal class ActivityUserPermissionsFilter
    {
        public List<long> DepartmentIds { get; set; }
    }

    internal class ActivityActivitiesFilter
    {
        public List<long> ActivityIds { get; set; }
    }

    internal class ActivitySpecificObjectivesFilter
    {
        public List<long> SpecificObjectiveIds { get; set; }
    }

    internal class ActivityFunctionariesFilter
    {
        public List<long> FunctionaryIds { get; set; }
    }

    internal class ActivityFilterBuilder : DataExpressionFilterBuilder<Activity, ActivitiesPermissionsFilter>
    {
        public ActivityFilterBuilder(IServiceProvider serviceProvider, ActivitiesPermissionsFilter filter)
            : base(serviceProvider, filter)
        {
        }

        protected override void InternalBuild()
        {
            if (EntityFilter.UserPermissionsFilter != null)
            {
                BuildUserPermissionsFilter();
            }

            if (EntityFilter.SpecificObjectivesFilter != null && EntityFilter.SpecificObjectivesFilter.SpecificObjectiveIds.Any())
            {
                BuildSpecificObjectivesFilter();
            }

            if (EntityFilter.FunctionariesFilter != null && EntityFilter.FunctionariesFilter.FunctionaryIds.Any())
            {
                BuildFunctionariesFilter();
            }

            if (EntityFilter.ActivitiesFilter != null && EntityFilter.ActivitiesFilter.ActivityIds.Any())
            {
                BuildActivitiesFilter();
            }
        }

        private void BuildUserPermissionsFilter()
        {
            var departmentIds = EntityFilter.UserPermissionsFilter.DepartmentIds;

            EntityPredicates.Add(x => departmentIds.Contains(x.DepartmentId));
        }

        private void BuildSpecificObjectivesFilter()
        {
            var specificObjectiveIds = EntityFilter.SpecificObjectivesFilter.SpecificObjectiveIds;

            EntityPredicates.Add(x => specificObjectiveIds.Contains(x.SpecificObjectiveId));
        }

        private void BuildActivitiesFilter()
        {
            var activityIds = EntityFilter.ActivitiesFilter.ActivityIds;

            EntityPredicates.Add(x => activityIds.Contains(x.Id));
        }

        private void BuildFunctionariesFilter()
        {
            var functionaryIds = EntityFilter.FunctionariesFilter.FunctionaryIds;

            EntityPredicates.Add(x => x.ActivityFunctionarys.Any(y => functionaryIds.Contains(y.FunctionaryId)));
        }
    }
}
