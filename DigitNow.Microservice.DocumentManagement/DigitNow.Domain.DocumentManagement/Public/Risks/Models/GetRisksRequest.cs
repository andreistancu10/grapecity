using DigitNow.Domain.DocumentManagement.Contracts.Risks.Enums;

namespace DigitNow.Domain.DocumentManagement.Public.Risks.Models
{
    public class GetRisksRequest
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public RiskFilterDto Filter { get; set; }
    }

    public class RiskFilterDto
    {
        public GeneralObjectiveFilterDto GeneralObjectiveFilter { get; set; }
        public SpecificObjectiveFilterDto SpecificObjectiveFilter { get; set; }
        public ActivityFilterDto ActivityFilter { get; set; }
        public RiskNameFilterDto RiskNameFilter { get; set; }
        public DepartmentFilterDto DepartmentFilter { get; set; }
        public DateOfLastRevisionFilterDto LastRevisionFilter { get; set; }
        public ProbabilityOfApparitionFilterDto ProbabilityOfApparitionFilter { get; set; }
        public ImpactOfObjectivesEstimationFilterDto ImpactOfObjectivesEstimationFilter { get; set; }
    }

    public class GeneralObjectiveFilterDto
    {
        public long GeneralObjectiveId { get; set; }
    }

    public class SpecificObjectiveFilterDto
    {
        public long SpecificObjectiveId { get; set; }
    }

    public class ActivityFilterDto
    {
        public long ActivityId { get; set; }
    }

    public class RiskNameFilterDto
    {
        public string RiskName { get; set; }
    }

    public class DepartmentFilterDto
    {
        public long DepartmentId { get; set; }
    }

    public class DateOfLastRevisionFilterDto
    {
        public DateTime DateOfLastRevision { get; set; }
    }

    public class ProbabilityOfApparitionFilterDto
    {
        public RiskProbability ProbabilityOfApparitionEstimation { get; set; }
    }

    public class ImpactOfObjectivesEstimationFilterDto
    {
        public RiskProbability ImpactOfObjectivesEstimation { get; set; }
    }
}
