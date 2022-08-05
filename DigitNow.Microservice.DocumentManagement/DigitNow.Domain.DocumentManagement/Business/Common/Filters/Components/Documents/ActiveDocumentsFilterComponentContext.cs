using DigitNow.Domain.DocumentManagement.Data.Filters.Documents;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Documents
{
    internal class ActiveDocumentsFilterComponentContext : DataExpressionFilterComponentContext
    {
        public DocumentFilter DocumentFilter { get; set; }
    }
}
