using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Contracts.Risks.Enums;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Risks
{
    public class Risk : ExtendedEntity
    {
        public long GeneralObjectiveId { get; set; }
        public long SpecificObjectiveId { get; set; }
        public long ActivityId { get; set; }
        public long? ActionId { get; set; }
        public long DepartmentId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public ScimState State { get; set; }
        public string RiskCauses { get; set; }
        public string RiskConsequences { get; set; }
        public RiskProbability ProbabilityOfApparitionEstimation { get; set; }
        public RiskProbability ImpactOfObjectivesEstimation { get; set; }
        public string RiskExposureEvaluation { get; set; }
        public HeadOfDepartmentDecision HeadOfDepartmentDecision { get; set; }
        public string? HeadOfDepartmentAssignation { get; set; }
        public AdoptedStrategy AdoptedStrategy { get; set; }
        public string? AdoptedStrategyAssignation { get; set; }
        public string StrategyDetails { get; set; }
        public string? UtilizedDocumentation { get; set; }

        public List<RiskControlAction> RiskControlActions { get; set; }

        public GeneralObjective AssociatedGeneralObjective { get; set; }
        public SpecificObjective AssociatedSpecificObjective { get; set; }
        public Activity AssociatedActivity { get; set; }
        public Action AssociatedAction { get; set; }
    }
}
