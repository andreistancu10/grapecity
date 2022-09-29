namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class RiskActionProposal : ExtendedEntity
    {
        public long RiskTrackingReportId { get; set; }
        public string ProposedAction { get; set; }
        public string Deadline { get; set; }
        public string RiskTrackingReportDate { get; set; }
    }
}
