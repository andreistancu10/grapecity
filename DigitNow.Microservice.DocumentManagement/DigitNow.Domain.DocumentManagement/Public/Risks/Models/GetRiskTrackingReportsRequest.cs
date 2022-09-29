namespace DigitNow.Domain.DocumentManagement.Public.Risks.Models
{
    public class GetRiskTrackingReportsRequest
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
    }
}
