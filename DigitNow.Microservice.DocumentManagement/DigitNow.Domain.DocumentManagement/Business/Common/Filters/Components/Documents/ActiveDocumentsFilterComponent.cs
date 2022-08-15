using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Documents;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Documents
{
    internal class ActiveDocumentsFilterComponent : DataExpressionFilterComponent<Document, ActiveDocumentsFilterComponentContext>
    {
        #region [ Properties ]

        private static int PreviousYear => DateTime.UtcNow.Year - 1;

        #endregion

        #region [ Construction ]

        public ActiveDocumentsFilterComponent(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        #endregion

        #region [ Component Internals ]

        protected override Task<DataExpressions<Document>> SetBuiltinDataExpressionsAsync(ActiveDocumentsFilterComponentContext context, CancellationToken token)
        {
            return Task.FromResult(new DataExpressions<Document>
            {
                x => x.CreatedAt.Year >= PreviousYear,
                x => !x.IsArchived
            });
        }

        protected override Task<DataExpressions<Document>> SetCustomDataExpressionsAsync(ActiveDocumentsFilterComponentContext context, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<Document>();

            if (!context.DocumentFilter.IsEmpty())
            {
                var filterBuilder = new DocumentFilterBuilder(ServiceProvider, context.DocumentFilter);
                dataExpressions.AddRange(filterBuilder.Build());
            }

            return Task.FromResult(dataExpressions);
        }

        #endregion
    }
}
