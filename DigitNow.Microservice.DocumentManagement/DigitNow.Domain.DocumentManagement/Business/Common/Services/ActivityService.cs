using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Contracts.Scim;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Activities;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Activities;
using Microsoft.EntityFrameworkCore.Storage;

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
        private readonly ICatalogClient _catalogClient;

        public ActivityService(
            DocumentManagementDbContext dbContext,
            IServiceProvider serviceProvider,
            ICatalogClient catalogClient) : base(dbContext)
        {
            _serviceProvider = serviceProvider;
            _catalogClient = catalogClient;
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

        public async Task<Activity> AddAsync(Activity activity, CancellationToken cancellationToken)
        {
            var scimStates = await _catalogClient.ScimStates.GetScimStatesAsync(cancellationToken);
            
            //TODO how do I know which is the correct value
            activity.StateId = ScimState.Active; 
            IDbContextTransaction dbContextTransaction = null;

            try
            {
                dbContextTransaction = await DbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
                DbContext.Entry(activity).State = EntityState.Added;
                await SetActivityCodeAsync(activity, cancellationToken);
                await DbContext.SaveChangesAsync(cancellationToken);
                await dbContextTransaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await dbContextTransaction?.RollbackAsync(cancellationToken);
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
            await ChangeStateAsync(new List<long> { activity.Id }, ScimEntity.ScimActivity, activity.StateId, cancellationToken);
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


        private async Task<DataExpressions<Activity>> GetActivitiesDataExpressions(ActivityFilter filter, UserModel currentUser, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<Activity>();

            dataExpressions.AddRange(await GetActivitiesExpressionsAsync(filter, token));
            dataExpressions.AddRange(await GetActivitiesUserRightsExpressionsAsync(currentUser, token));

            return dataExpressions;
        }

        private Task<DataExpressions<Activity>> GetActivitiesUserRightsExpressionsAsync(UserModel currentUser, CancellationToken token)
        {
            var rightsComponent = new ActivitiesPermissionsFilterComponent(_serviceProvider);
            var rightsComponentContext = new ActivitiesPermissionsFilterComponentContext
            {
                CurrentUser = currentUser
            };

            return rightsComponent.ExtractDataExpressionsAsync(rightsComponentContext, token);
        }

        private Task<DataExpressions<Activity>> GetActivitiesExpressionsAsync(ActivityFilter filter, CancellationToken token)
        {
            var activitiesFilterComponent = new ActivitiesFilterComponent(_serviceProvider);

            var activitiesFilterComponentContext = new ActivitiesFilterComponentContext
            {
                ActivityFilter = filter
            };

            return activitiesFilterComponent.ExtractDataExpressionsAsync(activitiesFilterComponentContext, token);
        }

        private IQueryable<Activity> GetBuiltInActivitiesQuery()
        {
            return DbContext.Activities
                .Include(c => c.ActivityFunctionaries)
                .Include(x => x.AssociatedGeneralObjective);
        }
    }
}
