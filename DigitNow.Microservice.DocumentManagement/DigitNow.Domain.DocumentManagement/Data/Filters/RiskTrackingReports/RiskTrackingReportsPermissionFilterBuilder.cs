using DigitNow.Domain.DocumentManagement.Data.Entities.Risks;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.RiskTrackingReports
{
    internal class RiskTrackingReportsPermissionFilterBuilder : DataExpressionFilterBuilder<RiskTrackingReport, RiskTrackingReportsPermissionFilter>
    {
        public RiskTrackingReportsPermissionFilterBuilder(IServiceProvider serviceProvider, RiskTrackingReportsPermissionFilter filter)
            : base(serviceProvider, filter) { }

        protected override void InternalBuild()
        {
            if (EntityFilter.UserPermissionsFilter != null)
            {
                BuildUserPermissionsFilter();
            }
        }

        private void BuildUserPermissionsFilter()
        {
            var departments = EntityFilter.UserPermissionsFilter.DepartmentIds;

            EntityPredicates.Add(x => departments.Contains(x.Risk.DepartmentId));
        }
    }
}
