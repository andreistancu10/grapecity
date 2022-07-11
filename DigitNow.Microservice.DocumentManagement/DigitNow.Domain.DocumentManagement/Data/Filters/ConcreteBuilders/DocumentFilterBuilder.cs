using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters
{
    internal interface IDocumentFilterBuilder : IExpressionFilterBuilder<Document, DocumentFilter>
    {
        void BuildFilterByRegistryType();
        void BuildFilterByRegistrationNo();
        void BuildFilterByRegistrationDate();
        void BuildFilterByDocumentType();
        void BuildFilterByDocumentState();
        void BuildFilterByIdentifiers();
    }

    internal class DocumentFilterBuilder : ExpressionFilterBuilder<Document, DocumentFilter>, IDocumentFilterBuilder
    {
        public DocumentFilterBuilder(DocumentFilter documentFilterModel)
            : base(documentFilterModel) { }

        public void BuildFilterByRegistryType()
        {
            if (EntityFilter.RegistryTypeFilter != null)
            {
                //TODO: Ask about this
                GeneratedFilters.Add(document => document != null);
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
            if (EntityFilter.DocumentIdentifiersFilter != null)
            {
                GeneratedFilters.Add(document => EntityFilter.DocumentIdentifiersFilter.Identifiers.Contains(document.Id));
            }
        }

        protected override void InternalBuild()
        {
            if (EntityFilter.DocumentIdentifiersFilter != null)
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
            }
        }
    }
}
