using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Documents.Preprocess;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components
{
    internal class DocumentPreprocessFilterComponent : DataExpressionFilterComponent<Document, DocumentPreprocessFilterComponentContext>
    {
        private readonly DocumentPreprocessFilterBuilder _preprocessFilterBuilder;

        public DocumentPreprocessFilterComponent(
            IServiceProvider serviceProvider,
            DocumentPreprocessFilter filter)
            : base(serviceProvider)
        {
            _preprocessFilterBuilder = new DocumentPreprocessFilterBuilder(serviceProvider, filter);
        }

        public override DataExpressions<Document> ExtractDataExpressions(DocumentPreprocessFilterComponentContext context)
        {
            return _preprocessFilterBuilder.Build();
        }            
    }
}
