using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    public class ReportViewModelAggregate
    {
        public ReportViewModel ReportViewModel { get; set; }
        public Dictionary<DocumentStatus, string> StatusTranslations { get; set; }
        public Dictionary<DocumentType, string> DocumentTypeTranslations { get; set; }
    }
}