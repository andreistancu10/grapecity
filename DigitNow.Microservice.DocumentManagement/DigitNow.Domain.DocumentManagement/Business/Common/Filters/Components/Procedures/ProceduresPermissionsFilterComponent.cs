using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Procedures;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Procedures
{
    internal class ProceduresPermissionsFilterComponent: DataExpressionFilterComponent<Procedure, ProceduresPermissionsFilterComponentContext>
	{
		public ProceduresPermissionsFilterComponent(IServiceProvider serviceProvider) : base(serviceProvider) { }
		
		protected override Task<DataExpressions<Procedure>> SetCustomDataExpressionsAsync(ProceduresPermissionsFilterComponentContext context, CancellationToken token)
		{
			var currentUser = context.CurrentUser;

			var filter = new ProcedurePermissionFilter
			{
				UserPermissionsFilter = new ProceduresUserPermissionsFilters
				{
					DepartmentIds = currentUser.Departments.Select(x => x.Id).ToList()
				}
			};

			return Task.FromResult(new ProcedurePermissionFilterBuilder(ServiceProvider, filter)
				.Build());
		}
	}
}
