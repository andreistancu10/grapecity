namespace DigitNow.Domain.DocumentManagement.Data.Filters.Activities
{
    public class ActivityFilter : DataFilter
    {
        public SpecificObjectivesFilter SpecificObjectivesFilter { get; set; }
        public ActivitiesFilter ActivitiesFilter { get; set; }
        public DepartmentsFilter DepartmentsFilter { get; set; }
        public FunctionariesFilter FunctionariesFilter { get; set; }

        public static ActivityFilter Empty => new();
    }

    public class SpecificObjectivesFilter
    {
        public List<long> SpecificObjectiveIds { get; set; }
    }

    public class ActivitiesFilter
    {
        public List<long> ActivityIds { get; set; }
    }

    public class DepartmentsFilter
    {
        public List<long> DepartmentIds { get; set; }
    }

    public class FunctionariesFilter
    {
        public List<long> FunctionaryIds { get; set; }
    }
}