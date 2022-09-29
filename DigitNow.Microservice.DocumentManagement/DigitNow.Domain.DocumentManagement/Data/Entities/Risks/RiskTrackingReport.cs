using DigitNow.Domain.DocumentManagement.Contracts.Risks.Enums;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class RiskTrackingReport : ExtendedEntity
    {
        public long RiskId { get; set; }
        public string ControlMeasuresImplementationState { get; set; }
        public string Difficulties { get; set; }
        public RiskProbability ProbabilityOfApparitionEstimation { get; set; }
        public RiskProbability ImpactOfObjectivesEstimation { get; set; }
        public string RiskExposureEvaluation { get; set; }

        public List<RiskActionProposal> RiskActionProposals { get; set; } = new();

        #region Relationships
        public Risk Risk { get; set; }

        #endregion
    }
}
