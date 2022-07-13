using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteBuilders
{
    internal class DocumentPostprocessFilterBuilder<T> : ExpressionFilterBuilder<T, DocumentPostprocessFilter>
        where T : VirtualDocument
    {
        public DocumentPostprocessFilterBuilder(DocumentPostprocessFilter filter)
            : base(filter) { }

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
}
