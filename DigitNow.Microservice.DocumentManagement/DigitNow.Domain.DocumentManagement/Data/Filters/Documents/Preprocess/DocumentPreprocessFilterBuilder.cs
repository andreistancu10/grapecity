using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Documents.Preprocess
{
    internal class DocumentPreprocessFilterBuilder : DataExpressionFilterBuilder<Document, DocumentPreprocessFilter>
    {
        private readonly DocumentManagementDbContext _dbContext;

        public DocumentPreprocessFilterBuilder(IServiceProvider serviceProvider, DocumentPreprocessFilter filter)
            : base(serviceProvider, filter)
        {
            _dbContext = serviceProvider.GetService<DocumentManagementDbContext>();
        }

        private void BuildFilterByRegistryType()
        {
            if (EntityFilter.RegistryTypeFilter != null)
            {
                var foundSpecialRegistrerIds = _dbContext.SpecialRegisters
                    .Where(x => EntityFilter.RegistryTypeFilter.RegistryTypes.Contains(x.Name))
                    .Select(x => x.Id);

                var targetDocumentIds = _dbContext.SpecialRegisterMappings
                    .Where(x => foundSpecialRegistrerIds.Contains(x.SpecialRegisterId))
                    .Select(x => x.DocumentId);

                EntityPredicates.Add(document => targetDocumentIds.Contains(document.Id));
            }
        }

        private void BuildFilterByRegistrationNo()
        {
            var registrationNoFilter = EntityFilter.RegistrationNoFilter;
            if (registrationNoFilter != null)
            {
                EntityPredicates.Add(document =>
                    document.RegistrationNumber >= registrationNoFilter.From
                    &&
                    document.RegistrationNumber <= registrationNoFilter.To
                );
            }
        }

        private void BuildFilterByRegistrationDate()
        {
            var registrationDateFilter = EntityFilter.RegistrationDateFilter;
            if (registrationDateFilter != null)
            {
                EntityPredicates.Add(document =>
                    document.RegistrationDate >= registrationDateFilter.From
                    &&
                    document.RegistrationDate <= registrationDateFilter.To
                );
            }
        }

        private void BuildFilterByDocumentType()
        {
            if (EntityFilter.TypeFilter != null)
            {
                EntityPredicates.Add(document => document.DocumentType == EntityFilter.TypeFilter.DocumentType);
            }
        }

        private void BuildFilterByDocumentState()
        {
            if (EntityFilter.StatusFilter != null)
            {
                EntityPredicates.Add(document => document.Status == EntityFilter.StatusFilter.Status);
            }
        }

        private void BuildFilterByIdentifiers()
        {
            if (EntityFilter.IdentifiersFilter != null)
            {
                EntityPredicates.Add(document => EntityFilter.IdentifiersFilter.Identifiers.Contains(document.Id));
            }
        }

        private void BuildFilterByDepartment()
        {
            if (EntityFilter.DepartmentFilter != null)
            {
                EntityPredicates.Add(document => EntityFilter.DepartmentFilter.DepartmentIds.Contains(document.DestinationDepartmentId));
            }
        }

        protected override void InternalBuild()
        {
            if (EntityFilter.IdentifiersFilter != null)
            {
                BuildFilterByIdentifiers();
            }
            else
            {
                BuildFilterByRegistryType();
                BuildFilterByRegistrationNo();
                BuildFilterByRegistrationDate();
                BuildFilterByDocumentType();
                BuildFilterByDocumentState();
                BuildFilterByDepartment();
            }
        }
    }
}
