using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Public.Dashboard.Models;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters
{
    internal interface IDocumentExpressionFilterBuilder : IExpressionFilterBuilder<Document, DocumentFilter>
    {
        void BuildFilterByRegistryType();
        void BuildFilterByRegistrationNo();
        void BuildFilterByRegistrationDate();
        void BuildFilterByDocumentType();
        // Search in Catalog for DocumentType & DocumentTypeInternal
        void BuildFilterByDocumentCategory();
        void BuildFilterByDocumentState();
    }

    internal class DocumentExpressionFilterBuilder : ExpressionFilterBuilder<Document, DocumentFilter>, IDocumentExpressionFilterBuilder
    {
        public DocumentExpressionFilterBuilder(DocumentFilter documentFilterModel)
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

        // Search in Catalog for DocumentType & DocumentTypeInternal
        public void BuildFilterByDocumentCategory()
        {
            var documentTypeFilter = EntityFilter.TypeFilter;
            if (documentTypeFilter == null) return;

            if (documentTypeFilter.DocumentType == DocumentType.Incoming || documentTypeFilter.DocumentType == DocumentType.Outgoing)
            {
                // TODO: Move this after the data is retrieved
                GeneratedFilters.Add(document => document != null);
            }
            else if (documentTypeFilter.DocumentType != DocumentType.Internal)
            {
                // TODO: Move this after the data is retrieved
                GeneratedFilters.Add(document => document != null);
            }
        }

        public void BuildFilterByDocumentState()
        {
            if (EntityFilter.StatusFilter != null)
            {
                // TODO: Apply filter after the status is set to documents
                GeneratedFilters.Add(document => document != null);
            }
        }

        protected override void InternalBuild()
        {
            BuildFilterByRegistryType();
            BuildFilterByRegistrationNo();
            BuildFilterByRegistrationDate();
            BuildFilterByDocumentType();
            BuildFilterByDocumentCategory();
            BuildFilterByDocumentState();
        }
    }
}
