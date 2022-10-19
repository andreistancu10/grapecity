using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Standards
{
    internal class StandardPermissionFilterBuilder: DataExpressionFilterBuilder<Standard, StandardPermissionFilter>
    {
        public StandardPermissionFilterBuilder(IServiceProvider serviceProvider, StandardPermissionFilter filter)
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
