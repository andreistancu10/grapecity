using DigitNow.Adapters.MS.Catalog;
using DigitNow.Adapters.MS.Catalog.Poco;
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
            var registryOfficeDepartment = await GetRegistryOfficeDepartmentAsync(token);

            var containsRegistryOfficeDepartments = context.CurrentUser.Departments.Contains(registryOfficeDepartment.Id);
            if (containsRegistryOfficeDepartments)
            {
                return new DocumentDepartmentRightsFilterBuilder(ServiceProvider, context.DepartmentRightsFilter)
                    .Build();
            }

            return new DocumentUserRightsFilterBuilder(ServiceProvider, context.UserRightsFilter)
                .Build();
        }

        #endregion

        #region [ Helpers ]

        private async Task<DepartmentDto> GetRegistryOfficeDepartmentAsync(CancellationToken token)
        {
            //TODO: Get departments by code
            var departments = await _catalogAdapterClient.GetDepartmentsAsync(token);

            return departments.First(x => x.Name == "Registratura");
        }

        #endregion
    }
}
