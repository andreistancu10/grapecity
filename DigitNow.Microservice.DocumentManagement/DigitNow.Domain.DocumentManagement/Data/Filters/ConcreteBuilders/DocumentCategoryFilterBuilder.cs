using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFiltersData;
using System.Collections.Generic;
using System.Linq;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteBuilders
{
    internal class DocumentCategoryFilterBuilder<T> : ExpressionFilterBuilder<T, DocumentFilter>
        where T : VirtualDocument
    {
        private readonly IList<long> _categoriesIds;
        private readonly IList<long> _internalCategoriesIds;

        public DocumentCategoryFilterBuilder(DocumentFilter filter, IList<long> categoriesIds, IList<long> internalCategoriesIds)
            : base(filter)
        {
            _categoriesIds = categoriesIds;
            _internalCategoriesIds = internalCategoriesIds;
        }

        private void BuildFilterByCategory()
        {
            if (EntityFilter == null || EntityFilter.CategoryFilter == null)
                return;

            var categoriesIds = EntityFilter.CategoryFilter.CategoryIds;

            if (typeof(T) == typeof(InternalDocument))
            {
                GeneratedFilters.Add(x => categoriesIds.Contains(((InternalDocument)(object)x).InternalDocumentTypeId));
                return;
            }

            if (typeof(T) == typeof(IncomingDocument))
            {
                GeneratedFilters.Add(x => categoriesIds.Contains(((IncomingDocument)(object)x).DocumentTypeId));
                return;
            }

            if (typeof(T) == typeof(OutgoingDocument))
            {
                GeneratedFilters.Add(x => categoriesIds.Contains(((OutgoingDocument)(object)x).DocumentTypeId));
                return;
            }
        }

        protected override void InternalBuild()
        {
            BuildFilterByCategory();
        }
    }


    //internal class InternalDocumentCategoryFilterBuilder : ExpressionFilterBuilder<InternalDocument, DocumentFilter>
    //{
    //    private readonly IList<DocumentCategoryFilterData> _categories;

    //    public InternalDocumentCategoryFilterBuilder(DocumentFilter filter, IList<DocumentCategoryFilterData> categories)
    //        : base(filter)
    //    {
    //        _categories = categories;
    //    }

    //    private void BuildFilterByCategory()
    //    {
    //        var categoriesIds = _categories.Select(x => x.Id);
    //        GeneratedFilters.Add(x => categoriesIds.Contains(x.InternalDocumentTypeId));
    //    }

    //    protected override void InternalBuild()
    //    {
    //        BuildFilterByCategory();
    //    }
    //}

    //internal class IncommingDocumentCategoryFilterBuilder : ExpressionFilterBuilder<IncomingDocument, DocumentFilter>
    //{
    //    private readonly IList<DocumentCategoryFilterData> _categories;

    //    public IncommingDocumentCategoryFilterBuilder(DocumentFilter filter, IList<DocumentCategoryFilterData> categories)
    //        : base(filter)
    //    {
    //        _categories = categories;
    //    }

    //    private void BuildFilterByCategory()
    //    {
    //        var categoriesIds = _categories.Select(x => x.Id);
    //        GeneratedFilters.Add(x => categoriesIds.Contains(x.DocumentTypeId));
    //    }

    //    protected override void InternalBuild()
    //    {
    //        BuildFilterByCategory();
    //    }
    //}

    //internal class OutgoingDocumentCategoryFilterBuilder : ExpressionFilterBuilder<IncomingDocument, DocumentFilter>
    //{
    //    private readonly IList<DocumentCategoryFilterData> _categories;

    //    public OutgoingDocumentCategoryFilterBuilder(DocumentFilter filter, IList<DocumentCategoryFilterData> categories)
    //        : base(filter)
    //    {
    //        _categories = categories;
    //    }

    //    private void BuildFilterByCategory()
    //    {
    //        var categoriesIds = _categories.Select(x => x.Id);
    //        GeneratedFilters.Add(x => categoriesIds.Contains(x.DocumentTypeId));
    //    }

    //    protected override void InternalBuild()
    //    {
    //        BuildFilterByCategory();
    //    }
    //}
}
