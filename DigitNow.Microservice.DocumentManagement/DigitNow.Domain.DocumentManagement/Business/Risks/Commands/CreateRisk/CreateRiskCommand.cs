using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
using DigitNow.Domain.DocumentManagement.Contracts.Risks.Enums;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Risks.Commands.CreateRisk
{
    public class CreateRiskCommand : ICommand<ResultObject>
    {
        public long GeneralObjectiveId { get; set; }
        public long SpecificObjectiveId { get; set; }
        public long ActivityId { get; set; }
        public long? ActionId { get; set; }
        public long DepartmentId { get; set; }
        public string Description { get; set; }
        public string RiskCauses { get; set; }
        public string RiskConsequences { get; set; }
        public RiskProbability ProbabilityOfApparitionEstimation { get; set; }
        public RiskProbability ImpactOfObjectivesEstimation { get; set; }
        public HeadOfDepartmentDecision HeadOfDepartmentDecision { get; set; }
        public string? HeadOfDepartmentAssignation { get; set; }
        public AdoptedStrategy AdoptedStrategy { get; set; }
        public string? AdoptedStrategyAssignation { get; set; }
        public string StrategyDetails { get; set; }
        public string? UtilizedDocumentation { get; set; }

        public List<RiskControlActionDto> RiskControlActions { get; set; } = new();
        public List<long> UploadedFileIds { get; set; }
    }
}
