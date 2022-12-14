using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.SpecificObjectivesPermissions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Objectives
{
    internal class SpecificObjectivesPermissionsFilterComponent : DataExpressionFilterComponent<SpecificObjective, SpecificObjectivesPermissionsFilterComponentContext>
    {
        public SpecificObjectivesPermissionsFilterComponent(IServiceProvider serviceProvider)
           : base(serviceProvider)
        {
        }

        protected override Task<DataExpressions<SpecificObjective>> SetCustomDataExpressionsAsync(SpecificObjectivesPermissionsFilterComponentContext context, CancellationToken token)
        {
            var currentUser = context.CurrentUser;

            var filter = new SpecificObjectivePermissionsFilter
            {
                UserPermissionsFilter = new ObjectivesUserPermissionsFilters
                {
                    DepartmentIds = currentUser.Departments.Select(x => x.Id).ToList(),
                    UserId = currentUser.Id
                }
            };

            return Task.FromResult(new SpecificObjectivePermissionsFilterBuilder(ServiceProvider, filter)
                .Build());
        }
    }
}
