using DigitNow.Domain.DocumentManagement.Data.Filters;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Activities
{
    internal class ActivitiesPermissionsFilter : DataFilter
    {
        public ActivitySpecificObjectivesFilter SpecificObjectivesFilter { get; set; }
        public ActivityActivitiesFilter ActivitiesFilter { get; set; }
        public ActivityFunctionariesFilter FunctionariesFilter { get; set; }
        public ActivityUserPermissionsFilter UserPermissionsFilter { get; set; }
    }
}