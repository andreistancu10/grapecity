using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteBuilders;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteBuilders.Preprocess;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;

namespace DigitNow.Domain.DocumentManagement.Data.Filters
{
    internal static class ExpressionFilterBuilderRegistry
    {
        public static IExpressionGenericFilterBuilder<Document> GetDocumentPreprocessFilterBuilder(DocumentPreprocessFilter preprocessFilter) =>
            new DocumentPreprocessFilterBuilder(preprocessFilter);

        public static IExpressionGenericFilterBuilder<T> GetDocumentPostprocessFilterBuilder<T>(DocumentPostprocessFilter postprocessFilter)
                where T : VirtualDocument => new DocumentPostprocessFilterBuilder<T>(postprocessFilter);            
    }
}
