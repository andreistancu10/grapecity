using DigitNow.Domain.DocumentManagement.Contracts.Risks.Enums;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Dtos
{
    public class RiskTrackingReportDto
    {
        public string ControlMeasuresImplementationState { get; set; }
        public string Difficulties { get; set; }
        public RiskProbability ProbabilityOfApparitionEstimation { get; set; }
        public RiskProbability ImpactOfObjectivesEstimation { get; set; }
        public string RiskExposureEvaluation { get; set; }

        public List<RiskActionProposalDto> RiskActionProposals { get; set; } = new();
    }
}
