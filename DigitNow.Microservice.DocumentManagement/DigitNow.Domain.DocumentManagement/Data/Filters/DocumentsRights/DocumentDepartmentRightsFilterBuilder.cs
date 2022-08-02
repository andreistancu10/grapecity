using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.DocumentsRights
{
    internal class DocumentDepartmentRightsFilterBuilder : DataExpressionFilterBuilder<Document, DocumentDepartmentRightsFilter>
    {
        public DocumentDepartmentRightsFilterBuilder(IServiceProvider serviceProvider, DocumentDepartmentRightsFilter filter)
            : base(serviceProvider, filter) { }

        private void BuildRegistryOfficeFilter()
        {
            if (EntityFilter.RegistryOfficeFilter != null)
            {
                var registryOfficeId = EntityFilter.RegistryOfficeFilter.DepartmentId;

                // Incoming => Allow all
                // Internal => No not allow
                // Outgoing => Only with Status = Finalized

                EntityPredicates.Add(x =>
                    (x.DocumentType == DocumentType.Incoming && x.DestinationDepartmentId == registryOfficeId)
                    ||
                    (x.DocumentType == DocumentType.Outgoing && x.DestinationDepartmentId == registryOfficeId && x.Status == DocumentStatus.Finalized)
                );
            }
        }

        protected override void InternalBuild()
        {
            BuildRegistryOfficeFilter();
        }
    }
}
