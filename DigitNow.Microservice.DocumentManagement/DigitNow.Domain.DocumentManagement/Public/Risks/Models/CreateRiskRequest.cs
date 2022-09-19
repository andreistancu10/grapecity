using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
using DigitNow.Domain.DocumentManagement.Contracts.Risks.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.Risks.Models
{
    public class CreateRiskRequest
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

        public List<RiskControlActionDto> RiskControlActions { get; set; }
        public List<long> UploadedFileIds { get; set; }
    }
}
