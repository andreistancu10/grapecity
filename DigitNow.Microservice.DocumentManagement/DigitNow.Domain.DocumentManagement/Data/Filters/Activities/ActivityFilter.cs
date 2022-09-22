using DigitNow.Domain.DocumentManagement.Data.Filters.Common;

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
}