using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.Documents;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteBuilders;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteBuilders.Preprocess;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;

namespace DigitNow.Domain.DocumentManagement.Data.Filters
{
    internal static class ExpressionFilterBuilderRegistry
    {
        public static IExpressionGenericFilterBuilder<Document> GetDocumentPreprocessFilterBuilder(DocumentManagementDbContext dbContext, DocumentPreprocessFilter preprocessFilter) =>
            new DocumentPreprocessFilterBuilder(dbContext, preprocessFilter);

        public static IExpressionGenericFilterBuilder<T> GetDocumentPostprocessFilterBuilder<T>(DocumentManagementDbContext dbContext, DocumentPostprocessFilter postprocessFilter)
                where T : VirtualDocument => new DocumentPostprocessFilterBuilder<T>(dbContext, postprocessFilter);            
    }
}
