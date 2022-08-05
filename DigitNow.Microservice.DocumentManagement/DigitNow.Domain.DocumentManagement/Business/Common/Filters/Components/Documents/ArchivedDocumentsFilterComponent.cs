using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Documents;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Documents
{
    internal class ArchivedDocumentsFilterComponent : DataExpressionFilterComponent<Document, ArchivedDocumentsFilterComponentContext>
    {
        #region [ Properties ]

        private DateTime StartOfYear => new DateTime(DateTime.Now.Year, 1, 1);

        #endregion

        #region [ Construction ]

        public ArchivedDocumentsFilterComponent(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        #endregion

        #region [ Component Internals ]

        protected override Task<DataExpressions<Document>> SetBuiltinDataExpressionsAsync(ArchivedDocumentsFilterComponentContext context, CancellationToken token)
        {
            var builtinExpressions = new DataExpressions<Document>
            {
                x => x.IsArchived
            };

            if (context.DocumentFilter.RegistrationDateFilter == null)
            {
                builtinExpressions.Add(x => x.CreatedAt >= StartOfYear);
            }

            return Task.FromResult(builtinExpressions);
        }

        protected override Task<DataExpressions<Document>> SetCustomDataExpressionsAsync(ArchivedDocumentsFilterComponentContext context, CancellationToken token)
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
