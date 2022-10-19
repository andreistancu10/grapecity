using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.PublicAcquisitions
{
    internal class PublicAcquisitionPermissionFilterBuilder : DataExpressionFilterBuilder<PublicAcquisitionProject, PublicAcquisitionPermissionFilter>
    {
        public PublicAcquisitionPermissionFilterBuilder(IServiceProvider serviceProvider, PublicAcquisitionPermissionFilter filter)
            : base(serviceProvider, filter) { }
        private void BuildUserPermissionsFilter()
        {
            if (EntityFilter.UserPermissionsFilter == null)
                return;

            var userDepartmentIds = EntityFilter.UserPermissionsFilter.UserDepartmentIds;
            var allowedDepartmentId = EntityFilter.UserPermissionsFilter.AllowedDepartmentId;

            EntityPredicates.Add(x => userDepartmentIds.Contains(allowedDepartmentId));
        }

        protected override void InternalBuild()
        {
            BuildUserPermissionsFilter();
        }
    }
}
