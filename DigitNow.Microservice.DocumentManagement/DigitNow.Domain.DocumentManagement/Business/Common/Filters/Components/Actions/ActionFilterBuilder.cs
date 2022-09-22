using DigitNow.Domain.DocumentManagement.Data.Filters;
using Action = DigitNow.Domain.DocumentManagement.Data.Entities.Action;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Actions
{
    internal class ActionFilterBuilder : DataExpressionFilterBuilder<Action, ActionPermissionsFilter>
    {
        public ActionFilterBuilder(IServiceProvider serviceProvider, ActionPermissionsFilter filter)
            : base(serviceProvider, filter)
        {
        }

        protected override void InternalBuild()
        {
            if (EntityFilter.DepartmentsFilter != null)
            {
                BuildUserPermissionsFilter();
            }

            if (EntityFilter.SpecificObjectivesFilter != null && EntityFilter.SpecificObjectivesFilter.SpecificObjectiveIds.Any())
            {
                BuildSpecificObjectivesFilter();
            }

            if (EntityFilter.FunctionariesFilter != null && EntityFilter.FunctionariesFilter.FunctionaryIds.Any())
            {
                BuildFunctionariesFilter();
            }

            if (EntityFilter.ActionsFilter != null && EntityFilter.ActionsFilter.ActionIds.Any())
            {
                BuildActionsFilter();
            }

            if (EntityFilter.ActivitiesFilter != null && EntityFilter.ActivitiesFilter.ActivityIds.Any())
            {
                BuildActivitiesFilter();
            }
        }

        private void BuildUserPermissionsFilter()
        {
            var departmentIds = EntityFilter.DepartmentsFilter.DepartmentIds;

            EntityPredicates.Add(x => departmentIds.Contains(x.DepartmentId));
        }

        private void BuildSpecificObjectivesFilter()
        {
            var specificObjectiveIds = EntityFilter.SpecificObjectivesFilter.SpecificObjectiveIds;

            EntityPredicates.Add(x => specificObjectiveIds.Contains(x.AssociatedActivity.SpecificObjectiveId));
        }

        private void BuildActionsFilter()
        {
            var activityIds = EntityFilter.ActionsFilter.ActionIds;

            EntityPredicates.Add(x => activityIds.Contains(x.Id));
        }

        private void BuildActivitiesFilter()
        {
            var activityIds = EntityFilter.ActivitiesFilter.ActivityIds;

            EntityPredicates.Add(x => activityIds.Contains(x.Id));
        }

        private void BuildFunctionariesFilter()
        {
            var functionaryIds = EntityFilter.FunctionariesFilter.FunctionaryIds;

            EntityPredicates.Add(x => x.ActionFunctionaries.Any(y => functionaryIds.Contains(y.FunctionaryId)));
        }
    }
}