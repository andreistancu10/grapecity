using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Activities;
using DigitNow.Domain.DocumentManagement.Data.Filters;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Actions
{
    internal class ActionPermissionsFilter : DataFilter
    {
        public ActionSpecificObjectivesFilter SpecificObjectivesFilter { get; set; }
        public ActionActionsFilter ActionsFilter { get; set; }
        public ActionFunctionariesFilter FunctionariesFilter { get; set; }
        public ActionUserPermissionsFilter UserPermissionsFilter { get; set; }
    }
}