using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFiltersData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteBuilders
{
    internal interface IDocumentCategoryFilterBuilder : IExpressionFilterBuilder<VirtualDocument, DocumentFilter>
    {
        void BuildFilterByCategory();
        void BuildFilterByInternalCategory();
    }

    internal class DocumentCategoryFilterBuilder : ExpressionFilterBuilder<VirtualDocument, DocumentFilter>, IDocumentCategoryFilterBuilder
    {
        private readonly IList<DocumentCategoryFilterData> _categories;
        private readonly IList<DocumentInternalCategoryFilterData> _internalCategories;

        public DocumentCategoryFilterBuilder(DocumentFilter documentFilter, IList<DocumentCategoryFilterData> categories, IList<DocumentInternalCategoryFilterData> internalCategories) 
            : base(documentFilter) 
        {
            _categories = categories;
            _internalCategories = internalCategories;
        }

        public void BuildFilterByCategory()
        {
            GeneratedFilters.Add(document => document.docu);
        }

        public void BuildFilterByInternalCategory()
        {

        }

        protected override void InternalBuild()
        {
            var documentTypeFilter = EntityFilter.TypeFilter;
            if (documentTypeFilter == null) return;

            if (documentTypeFilter.DocumentType == DocumentType.Incoming || documentTypeFilter.DocumentType == DocumentType.Outgoing)
            {
                BuildFilterByCategory();
            }
            else if (documentTypeFilter.DocumentType != DocumentType.Internal)
            {
                BuildFilterByInternalCategory();
            }
        }
    }
}
