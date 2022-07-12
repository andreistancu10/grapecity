using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteBuilders;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFiltersData;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Data.Filters
{
    internal static class ExpressionFilterBuilderRegistry
    {
        public static IExpressionGenericFilterBuilder<Document> GetDocumentPredicatesByFilter(DocumentFilter documentFilter) =>
            new DocumentFilterBuilder(documentFilter);

        //TODO: Fix this
        public static IExpressionGenericFilterBuilder<T> GetDocumentCategoryFilterBuilder<T>(DocumentFilter documentFilter, IList<DocumentCategoryFilterData> filterData)
                where T : VirtualDocument =>
            new DocumentCategoryFilterBuilder<T>(documentFilter, null, null);            
    }
}
