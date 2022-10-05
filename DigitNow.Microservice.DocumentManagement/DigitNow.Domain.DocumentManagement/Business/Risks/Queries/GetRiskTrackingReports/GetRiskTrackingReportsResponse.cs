using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;

namespace DigitNow.Domain.DocumentManagement.Business.Risks.Queries.GetRiskTrackingReports
{
    public class GetRiskTrackingReportsResponse
    {
        public long TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long TotalPages { get; set; }
        public IList<RiskTrackingReportViewModel> Items { get; set; }
    }
}
