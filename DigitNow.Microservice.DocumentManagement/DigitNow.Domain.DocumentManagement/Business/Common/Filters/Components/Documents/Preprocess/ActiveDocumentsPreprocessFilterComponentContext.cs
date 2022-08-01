using DigitNow.Domain.DocumentManagement.Data.Filters.Documents;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Preprocess
{
    internal class ActiveDocumentsPreprocessFilterComponentContext : DataExpressionFilterComponentContext
    {
        public DocumentFilter DocumentFilter { get; set; }        
    }
}
