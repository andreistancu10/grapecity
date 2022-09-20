using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Common;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Activities
{
    internal class ActivitiesPermissionsFilter : DataFilter
    {
        public SpecificObjectivesFilter SpecificObjectivesFilter { get; set; }
        public ActivitiesFilter ActivitiesFilter { get; set; }
        public FunctionariesFilter FunctionariesFilter { get; set; }
        public DepartmentsFilter DepartmentsFilter { get; set; }
    }
}