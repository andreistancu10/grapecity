using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Standards;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Standards
{
    internal class StandardsPermissionsFilterComponent : DataExpressionFilterComponent<Standard, StandardsPermissionsFilterComponentContext>
    {
        public StandardsPermissionsFilterComponent(IServiceProvider serviceProvider) : base(serviceProvider) { }
		protected override Task<DataExpressions<Standard>> SetCustomDataExpressionsAsync(StandardsPermissionsFilterComponentContext context, CancellationToken token)
		{
			var currentUser = context.CurrentUser;

			var filter = new StandardPermissionFilter
			{
				UserPermissionsFilter = new StandardsUserPermissionsFilters
				{
					DepartmentIds = currentUser.Departments.Select(x => x.Id).ToList()
				}
			};

			return Task.FromResult(new StandardPermissionFilterBuilder(ServiceProvider, filter)
				.Build());
		}
	}
}
