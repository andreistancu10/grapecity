using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;
using System.Linq;
using DigitNow.Domain.DocumentManagement.Data.Entities.Documents;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteBuilders.Preprocess
{
    internal interface IDocumentPreprocessFilterBuilder : IExpressionFilterBuilder<Document, DocumentPreprocessFilter>
    {
        void BuildFilterByRegistryType();
        void BuildFilterByRegistrationNo();
        void BuildFilterByRegistrationDate();
        void BuildFilterByDocumentType();
        void BuildFilterByDocumentState();
        void BuildFilterByIdentifiers();
    }

    internal class DocumentPreprocessFilterBuilder : ExpressionFilterBuilder<Document, DocumentPreprocessFilter>, IDocumentPreprocessFilterBuilder
    {
        private readonly DocumentManagementDbContext _dbContext;

        public DocumentPreprocessFilterBuilder(DocumentManagementDbContext dbContext, DocumentPreprocessFilter documentFilterModel)
            : base(documentFilterModel)
        {
            _dbContext = dbContext;
        }

        public void BuildFilterByRegistryType()
        {
            if (EntityFilter.RegistryTypeFilter != null)
            {
                var foundSpecialRegistrerIds = _dbContext.SpecialRegisters
                    .Where(x => EntityFilter.RegistryTypeFilter.RegistryTypes.Contains(x.Name))
                    .Select(x => x.Id);

                var targetDocumentIds = _dbContext.SpecialRegisterMappings
                    .Where(x => foundSpecialRegistrerIds.Contains(x.SpecialRegisterId))
                    .Select(x => x.DocumentId);

                GeneratedFilters.Add(document => targetDocumentIds.Contains(document.Id));
            }
        }

        public void BuildFilterByRegistrationNo()
        {
            var registrationNoFilter = EntityFilter.RegistrationNoFilter;
            if (registrationNoFilter != null)
            {
                GeneratedFilters.Add(document =>
                    document.RegistrationNumber >= registrationNoFilter.From
                    &&
                    document.RegistrationNumber <= registrationNoFilter.To
                );
            }
        }

        public void BuildFilterByRegistrationDate()
        {
            var registrationDateFilter = EntityFilter.RegistrationDateFilter;
            if (registrationDateFilter != null)
            {
                GeneratedFilters.Add(document =>
                    document.RegistrationDate >= registrationDateFilter.From
                    &&
                    document.RegistrationDate <= registrationDateFilter.To
                );
            }
        }

        public void BuildFilterByDocumentType()
        {
            if (EntityFilter.TypeFilter != null)
            {
                GeneratedFilters.Add(document => document.DocumentType == EntityFilter.TypeFilter.DocumentType);
            }
        }

        public void BuildFilterByDocumentState()
        {
            if (EntityFilter.StatusFilter != null)
            {
                GeneratedFilters.Add(document => document.Status == EntityFilter.StatusFilter.Status);
            }
        }

        public void BuildFilterByIdentifiers()
        {
            if (EntityFilter.IdentifiersFilter != null)
            {
                GeneratedFilters.Add(document => EntityFilter.IdentifiersFilter.Identifiers.Contains(document.Id));
            }
        }

        public void BuildFilterByDepartment()
        {
            if (EntityFilter.DepartmentFilter != null)
            {
                GeneratedFilters.Add(document => EntityFilter.DepartmentFilter.DepartmentIds.Contains(document.DestinationDepartmentId));
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
