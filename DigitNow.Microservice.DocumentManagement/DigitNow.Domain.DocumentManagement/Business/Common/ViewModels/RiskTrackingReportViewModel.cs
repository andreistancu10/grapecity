using DigitNow.Domain.DocumentManagement.Contracts.Risks.Enums;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class RiskTrackingReportViewModel
    {
        public string ControlMeasuresImplementationState { get; set; }
        public string Difficulties { get; set; }
        public RiskProbability ProbabilityOfApparitionEstimation { get; set; }
        public RiskProbability ImpactOfObjectivesEstimation { get; set; }
        public string RiskExposureEvaluation { get; set; }
        public BasicViewModel CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<RiskActionProposalViewModel> RiskActionProposals { get; set; }
    }
}
