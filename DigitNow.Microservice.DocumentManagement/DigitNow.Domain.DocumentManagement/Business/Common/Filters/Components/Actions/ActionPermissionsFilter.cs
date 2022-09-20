using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Actions;
using DigitNow.Domain.DocumentManagement.Data.Filters.Common;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Actions
{
    internal class ActionPermissionsFilter : DataFilter
    {
        public SpecificObjectivesFilter SpecificObjectivesFilter { get; set; }
        public ActionsFilter ActionsFilter { get; set; }
        public ActivitiesFilter ActivitiesFilter { get; set; }
        public FunctionariesFilter FunctionariesFilter { get; set; }
        public DepartmentsFilter DepartmentsFilter { get; set; }
    }
}