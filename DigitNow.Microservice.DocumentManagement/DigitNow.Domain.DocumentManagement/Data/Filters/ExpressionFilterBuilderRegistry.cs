using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;

namespace DigitNow.Domain.DocumentManagement.Data.Filters
{
    internal static class ExpressionFilterBuilderRegistry
    {
        public static IExpressionGenericFilterBuilder<Document> GetDocumentPredicatesByFilter(DocumentFilter documentFilter) =>
            new DocumentFilterBuilder(documentFilter);
    }
}
