using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Documents.Postprocess
{
    internal class DocumentPostprocessFilterBuilder<T> : DataExpressionFilterBuilder<T, DocumentPostprocessFilter>
        where T : VirtualDocument
    {
        public DocumentPostprocessFilterBuilder(IServiceProvider serviceProvider, DocumentPostprocessFilter filter)
            : base(serviceProvider, filter) { }

        private void BuildFilterByCategory()
        {
            if (EntityFilter == null || EntityFilter.CategoryFilter == null)
                return;

            var categoriesIds = EntityFilter.CategoryFilter.CategoryIds;
            var targetType = typeof(T);

            if (targetType == typeof(InternalDocument))
            {
                EntityPredicates.Add(x => categoriesIds.Contains(((InternalDocument)(object)x).InternalDocumentTypeId));                
            }
            else if (targetType == typeof(IncomingDocument))
            {
                EntityPredicates.Add(x => categoriesIds.Contains(((IncomingDocument)(object)x).DocumentTypeId));
            }
            else if (targetType == typeof(OutgoingDocument))
            {
                EntityPredicates.Add(x => categoriesIds.Contains(((OutgoingDocument)(object)x).DocumentTypeId));
            }
        }

        protected override void InternalBuild()
        {
            BuildFilterByCategory();
        }
    }
}
