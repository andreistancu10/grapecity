using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Documents.Preprocess;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Preprocess
{
    internal class ActiveDocumentsPreprocessFilterComponent : DataExpressionFilterComponent<Document, ActiveDocumentsPreprocessFilterComponentContext>
    {
        #region [ Properties ]

        private static int PreviousYear => DateTime.UtcNow.Year - 1;

        #endregion

        #region [ Construction ]

        public ActiveDocumentsPreprocessFilterComponent(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        #endregion

        #region [ Component Internals ]

        protected override Task<DataExpressions<Document>> SetBuiltinDataExpressionsAsync(ActiveDocumentsPreprocessFilterComponentContext context, CancellationToken token)
        {
            return Task.FromResult(new DataExpressions<Document>
            {
                x => x.CreatedAt.Year >= PreviousYear
            });
        }

        protected override Task<DataExpressions<Document>> SetCustomDataExpressionsAsync(ActiveDocumentsPreprocessFilterComponentContext context, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<Document>();

            if (!context.DocumentFilter.PreprocessFilter.IsEmpty())
            {
                var filterBuilder = new DocumentPreprocessFilterBuilder(ServiceProvider, context.DocumentFilter.PreprocessFilter);
                dataExpressions.AddRange(filterBuilder.Build());
            }

            return Task.FromResult(dataExpressions);
        }

        #endregion
    }
}
