using DigitNow.Domain.DocumentManagement.Contracts.Risks.Enums;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Risks
{
    public class RiskFilter : DataFilter
    {
        public GeneralObjectiveFilter GeneralObjectiveFilter { get; set; }
        public SpecificObjectiveFilter SpecificObjectiveFilter { get; set; }
        public ActivityFilter ActivityFilter { get; set; }
        public RiskNameFilter RiskNameFilter { get; set; }
        public DepartmentFilter DepartmentFilter { get; set; }
        public DateOfLastRevisionFilter LastRevisionFilter { get; set; }
        public ProbabilityOfApparitionFilter ProbabilityOfApparitionFilter { get; set; }
        public ImpactOfObjectivesEstimationFilter ImpactOfObjectivesEstimationFilter { get; set; }

        public static RiskFilter Empty => new();
    }

    public class GeneralObjectiveFilter
    {
        public long GeneralObjectiveId { get; set; }
    }

    public class SpecificObjectiveFilter
    {
        public long SpecificObjectiveId { get; set; }
    }

    public class ActivityFilter
    {
        public long ActivityId { get; set; }
    }

    public class RiskNameFilter
    {
        public string RiskName { get; set; }
    }

    public class DepartmentFilter
    {
        public long DepartmentId { get; set; }
    }

    public class DateOfLastRevisionFilter
    {
        public DateTime DateOfLastRevision { get; set; }
    }

    public class ProbabilityOfApparitionFilter
    {
        public RiskProbability ProbabilityOfApparitionEstimation { get; set; }
    }

    public class ImpactOfObjectivesEstimationFilter
    {
        public RiskProbability ImpactOfObjectivesEstimation { get; set; }
    }
}
