using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Activities
{
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

        private static List<long> SetDepartmentsFilter(IReadOnlyCollection<long> departmentsFilterDepartmentIds, UserModel currentUser)
        {
            departmentsFilterDepartmentIds = departmentsFilterDepartmentIds.Any()
                ? currentUser.Departments.Select(c => c.Id).Intersect(departmentsFilterDepartmentIds).ToList()
                : currentUser.Departments.Select(c => c.Id).ToList();

            if (!departmentsFilterDepartmentIds.Any())
            {
                departmentsFilterDepartmentIds = currentUser.Departments.Select(c => c.Id).ToList();
            }

            return departmentsFilterDepartmentIds.ToList();
        }
    }
}