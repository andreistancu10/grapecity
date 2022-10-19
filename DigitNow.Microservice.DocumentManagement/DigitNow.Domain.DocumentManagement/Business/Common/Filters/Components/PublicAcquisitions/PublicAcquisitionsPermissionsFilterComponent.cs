using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.PublicAcquisitions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.PublicAcquisitions
{
    internal class PublicAcquisitionsPermissionsFilterComponent : DataExpressionFilterComponent<PublicAcquisitionProject, PublicAcquisitionsPermissionsFilterComponentContext>
    {
        public PublicAcquisitionsPermissionsFilterComponent(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override Task<DataExpressions<PublicAcquisitionProject>> SetCustomDataExpressionsAsync(PublicAcquisitionsPermissionsFilterComponentContext context, CancellationToken token)
        {
            var currentUser = context.CurrentUser;

            var filter = new PublicAcquisitionPermissionFilter
            {
                UserPermissionsFilter = new PublicAcquisitionPermissionsFilters
                {
                    UserDepartmentIds = currentUser.Departments.Select(x => x.Id).ToList(),
                    AllowedDepartmentId = context.AllowedDepartmentId
                }
            };

            return Task.FromResult(new PublicAcquisitionPermissionFilterBuilder(ServiceProvider, filter)
                .Build());
        }
    }
}
