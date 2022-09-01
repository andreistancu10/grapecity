using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Export;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.utils;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries.GetDocumentsReports
{
    public class GetDocumentsExcelReportsQuery : IQuery<List<ExportReportViewModel>>
    {
        public int LanguageId { get; set; } = LanguagesUtils.RomanianLanguageId;
        public ReportType Type { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}