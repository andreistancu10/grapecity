using DigitNow.Domain.DocumentManagement.Data.Entities.Risks;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Risks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Risks
{
    internal class RisksPermissionsFilterComponent : DataExpressionFilterComponent<Risk, RisksPermissionsFilterComponentContext>
    {
        public RisksPermissionsFilterComponent(IServiceProvider serviceProvider) : base(serviceProvider){}
        protected override Task<DataExpressions<Risk>> SetCustomDataExpressionsAsync(RisksPermissionsFilterComponentContext context, CancellationToken token)
        {
            var currentUser = context.CurrentUser;

            var filter = new RiskPermissionFilter
            {
                UserPermissionsFilter = new RisksUserPermissionsFilters
                {
                    DepartmentIds = currentUser.Departments.Select(x => x.Id).ToList()
                }
            };

            return Task.FromResult(new RiskPermissionFilterBuilder(ServiceProvider, filter)
                .Build());
        }
    }
}
