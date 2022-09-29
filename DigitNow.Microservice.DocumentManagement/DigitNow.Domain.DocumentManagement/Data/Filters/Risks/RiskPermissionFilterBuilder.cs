using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Risks
{
    internal class RiskPermissionFilterBuilder : DataExpressionFilterBuilder<Risk, RiskPermissionFilter>
    {
        public RiskPermissionFilterBuilder(IServiceProvider serviceProvider, RiskPermissionFilter filter)
            : base(serviceProvider, filter) { }

        protected override void InternalBuild()
        {
               BuildUserPermissionsFilter();
        }
        private void BuildUserPermissionsFilter()
        {
            if (EntityFilter.UserPermissionsFilter == null)
                return;

            var departments = EntityFilter.UserPermissionsFilter.DepartmentIds;
            EntityPredicates.Add(x => departments.Contains(x.DepartmentId));
        }
    }
}
