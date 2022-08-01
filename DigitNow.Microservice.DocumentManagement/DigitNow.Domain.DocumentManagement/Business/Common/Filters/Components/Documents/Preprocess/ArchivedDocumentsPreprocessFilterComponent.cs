using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Preprocess
{
    internal class ArchivedDocumentsPreprocessFilterComponent : DataExpressionFilterComponent<Document, ArchivedDocumentsFilterComponentContext>
    {
        public ArchivedDocumentsPreprocessFilterComponent(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        protected override Task<DataExpressions<Document>> SetCustomDataExpressionsAsync(ArchivedDocumentsFilterComponentContext context, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
