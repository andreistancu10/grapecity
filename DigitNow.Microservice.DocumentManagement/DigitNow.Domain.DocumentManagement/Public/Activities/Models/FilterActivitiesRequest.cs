using DigitNow.Domain.DocumentManagement.Contracts.Objectives;

namespace DigitNow.Domain.DocumentManagement.Public.Activities.Models
{
    public class FilterActivitiesRequest
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public ActivityFilterDto Filter { get; set; }
    }

    public class ActivityFilterDto
    {
        public ActivityChildPropertyIdsFilterDto ActivitySpecificObjectivesFilter { get; set; }
        public ActivityChildPropertyIdsFilterDto ActivityIdsFilter { get; set; }
        public ActivityChildPropertyIdsFilterDto ActivityDepartmentsFilter { get; set; }
        public ActivityChildPropertyIdsFilterDto ActivityFunctionariesFilter { get; set; }
    }

    public class ActivityChildPropertyIdsFilterDto
    {
        public List<long> Ids { get; set; }
    }

    public class ActivitySpecificObjectivesFilterDto
    {
        public DateTime GeneralObjectiveRegistrationDate { get; set; }
    }

    public class ActivityIdsFilterDto
    {
        public string GeneralObjectiveTitle { get; set; }
    }

    public class ActivityDepartmentsFilterDto
    {
        public string GeneralObjectiveCode { get; set; }
    }

    public class ActivityFunctionariesFilterDto
    {
        public ObjectiveState GeneralObjectiveState { get; set; }
    }
}
