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
    internal class DocumentPermissionsFilterComponent : DataExpressionFilterComponent<Document, DocumentPermissionsFilterComponentContext>
    {
        #region [ Fields ]

        private readonly ICatalogAdapterClient _catalogAdapterClient;

        #endregion

        #region [ Construction ]

        public DocumentPermissionsFilterComponent(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _catalogAdapterClient = serviceProvider.GetService<ICatalogAdapterClient>();
        }

        #endregion

        #region [ Component Internals ]

        protected override async Task<DataExpressions<Document>> SetCustomDataExpressionsAsync(DocumentPermissionsFilterComponentContext context, CancellationToken token)
        {
            var currentUser = context.CurrentUser;

            var filter = new DocumentPermissionsFilter
            {
                UserContextFilter = new DocumentUserContextFilter { DepartmentId = currentUser.Departments.First().Id },
                DepartmentsPermissionsFilter = await BuildDepartmentsPermissionsFilterAsync(context, token),
                UserPermissionsFilter = context.UserPermissionsFilter
            };

            return new DocumentPermissionsFilterBuilder(ServiceProvider, filter)
                .Build();
        }

        #endregion

        #region [ Helpers ]

        private async Task<DocumentDepartmentPermissionsFilters> BuildDepartmentsPermissionsFilterAsync(DocumentPermissionsFilterComponentContext context, CancellationToken token)
        {
            var registryOfficeDepartment = await GetRegistryOfficeDepartmentAsync(token);
            if (registryOfficeDepartment == null) throw new InvalidOperationException("Registry Office department was not found!");

            return new DocumentDepartmentPermissionsFilters
            {
                RegistryOfficeRightsFilter = new DocumentRegistryOfficeDepartmentRightsFilter
                {
                    DepartmentId = registryOfficeDepartment.Id,
                    UserId = context.CurrentUser.Id
                }
            };
        }

        [Obsolete("Will be refactored with the use of the Catalog SDK")]
        private Task<Department> GetRegistryOfficeDepartmentAsync(CancellationToken token) =>        
            _catalogAdapterClient.GetDepartmentByCodeAsync(UserDepartment.Registry.Code, token);

        #endregion
    }
}
