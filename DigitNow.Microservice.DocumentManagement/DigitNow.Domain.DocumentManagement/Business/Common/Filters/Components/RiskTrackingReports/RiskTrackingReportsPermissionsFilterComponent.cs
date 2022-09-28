using DigitNow.Domain.DocumentManagement.Data.Entities.Risks;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.RiskTrackingReports;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.RiskTrackingReports
{
    internal class RiskTrackingReportsPermissionsFilterComponent : DataExpressionFilterComponent<RiskTrackingReport, RiskTrackingReportsPermissionsFilterComponentContext>
    {
        public RiskTrackingReportsPermissionsFilterComponent(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override Task<DataExpressions<RiskTrackingReport>> SetCustomDataExpressionsAsync(RiskTrackingReportsPermissionsFilterComponentContext context, CancellationToken token)
        {
            var filter = new RiskTrackingReportsPermissionFilter
            {
                UserPermissionsFilter = new RiskTrackingReportsUserPermissionsFilters
                {
                    DepartmentIds = context.CurrentUser.Departments.Select(x => x.Id).ToList()
                }
            };

            return Task.FromResult(new RiskTrackingReportsPermissionFilterBuilder(ServiceProvider, filter)
                .Build());
        }
    }
}
