using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.utils;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries.GetDocumentsReports
{
    public class GetDocumentsReportsQuery : IQuery<List<ReportViewModel>>
    {
        public int LanguageId { get; set; } = LanguagesUtils.RomanianLanguageId;
        public ReportType Type { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}