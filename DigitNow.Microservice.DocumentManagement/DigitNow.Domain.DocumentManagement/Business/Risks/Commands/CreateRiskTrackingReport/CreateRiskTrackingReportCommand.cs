using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
using DigitNow.Domain.DocumentManagement.Contracts.Risks.Enums;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Risks.Commands.CreateRiskTrackingReport
{
    public class CreateRiskTrackingReportCommand : ICommand<ResultObject>
    {
        public long RiskId { get; set; }
        public string ControlMeasuresImplementationState { get; set; }
        public string Difficulties { get; set; }
        public RiskProbability ProbabilityOfApparitionEstimation { get; set; }
        public RiskProbability ImpactOfObjectivesEstimation { get; set; }
        public string RiskExposureEvaluation { get; set; }

        public List<RiskActionProposalDto> RiskActionProposals { get; set; }
        public List<long> UploadedFileIds { get; set; }
    }
}
