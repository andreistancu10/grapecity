using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Activities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Activities
{
    internal class ActivitiesPermissionsFilterComponent : DataExpressionFilterComponent<Activity, ActivitiesPermissionsFilterComponentContext>
    {
        public ActivitiesPermissionsFilterComponent(IServiceProvider serviceProvider) : base(serviceProvider) { }
        protected override Task<DataExpressions<Activity>> SetCustomDataExpressionsAsync(ActivitiesPermissionsFilterComponentContext context, CancellationToken token)
        {
            var currentUser = context.CurrentUser;

            var filter = new ActivitiesPermissionsFilter
            {
                UserPermissionsFilter = new ActivitiesUserPermissionsFilters
                {
                    DepartmentIds = currentUser.Departments.Select(x => x.Id).ToList()
                }
            };

            return Task.FromResult(new ActivitiesPermissionsFilterBuilder(ServiceProvider, filter)
                .Build());
        }
    }
}