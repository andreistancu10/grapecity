using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.DocumentsRights
{
    internal class DocumentDepartmentRightsFilterBuilder : DataExpressionFilterBuilder<Document, DocumentDepartmentRightsFilter>
    {
        private readonly DocumentManagementDbContext _dbContext;

        public DocumentDepartmentRightsFilterBuilder(IServiceProvider serviceProvider, DocumentDepartmentRightsFilter filter)
            : base(serviceProvider, filter)
        {
            _dbContext = serviceProvider.GetService<DocumentManagementDbContext>();
        }

        private void BuildRegistryOfficeFilter()
        {
            if (EntityFilter.RegistryOfficeFilter != null)
            {
                // Incoming => Allow all
                // Internal => No not allow
                // Outgoing => Only with Status = Finalized

                EntityPredicates.Add(x =>
                    x.DocumentType == DocumentType.Incoming
                    ||
                    
                        x.DocumentType == DocumentType.Outgoing
                        &&
                        x.Status == DocumentStatus.Finalized
                    
                );
            }
        }

        protected override void InternalBuild()
        {
            BuildRegistryOfficeFilter();
        }
    }
}
