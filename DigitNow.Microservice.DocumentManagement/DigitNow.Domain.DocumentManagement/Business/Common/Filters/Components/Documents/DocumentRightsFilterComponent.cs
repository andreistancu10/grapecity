using DigitNow.Adapters.MS.Catalog;
using DigitNow.Adapters.MS.Catalog.Poco;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.DocumentsRights;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Documents
{
    internal class DocumentRightsFilterPreprocessComponent : DataExpressionFilterComponent<Document, DocumentRightsFilterPreprocessComponentContext>
    {
        #region [ Fields ]

        private readonly ICatalogAdapterClient _catalogAdapterClient;

        #endregion

        #region [ Construction ]

        public DocumentRightsFilterPreprocessComponent(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _catalogAdapterClient = serviceProvider.GetService<ICatalogAdapterClient>();
        }

        #endregion

        #region [ Component Internals ]

        protected override async Task<DataExpressions<Document>> SetCustomDataExpressionsAsync(DocumentRightsFilterPreprocessComponentContext context, CancellationToken token)
        {
            var currentUser = context.CurrentUser;
            var result = new DataExpressions<Document>();

            // Mayor has full access
            if (IsRole(currentUser, RecipientType.Mayor)) 
                return result;

            var registryOfficeDepartment = await GetRegistryOfficeDepartmentAsync(token);

            // Set registry office department
            context.DepartmentRightsFilter.RegistryOfficeFilter.DepartmentId = registryOfficeDepartment.Id;

            var departmentFilters = new DocumentDepartmentRightsFilterBuilder(ServiceProvider, context.DepartmentRightsFilter).Build();
            result.AddRange(departmentFilters);

            var rolesFilters = new DocumentUserRightsFilterBuilder(ServiceProvider, context.UserRightsFilter).Build();
            result.AddRange(rolesFilters);

            return result;
        }

        #endregion

        #region [ Helpers ]

        private async Task<Department> GetRegistryOfficeDepartmentAsync(CancellationToken token)
        {
            //TODO: Get departments by code
            var department = await _catalogAdapterClient.GetDepartmentByCodeAsync("registratura",token);

            return department;
        }

        private static bool IsRole(UserModel userModel, RecipientType role) =>
            userModel.Roles.Contains(role.Code);

        #endregion
    }
}
