using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Risks.Queries.GetRiskTrackingReports
{
    public class GetRiskTrackingReportsQuery : IQuery<ResultObject>
    {
        public long RiskId { get; set; }
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
    }
}
