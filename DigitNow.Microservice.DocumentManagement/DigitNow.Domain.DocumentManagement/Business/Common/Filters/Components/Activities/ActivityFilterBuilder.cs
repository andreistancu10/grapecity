using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Activities
{
    internal class ActivityFilterBuilder : DataExpressionFilterBuilder<Activity, ActivitiesPermissionsFilter>
    {
        public ActivityFilterBuilder(IServiceProvider serviceProvider, ActivitiesPermissionsFilter filter)
            : base(serviceProvider, filter)
        {
        }

        protected override void InternalBuild()
        {
            if (EntityFilter.UserPermissionsFilter != null)
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

            if (EntityFilter.ActivitiesFilter != null && EntityFilter.ActivitiesFilter.ActivityIds.Any())
            {
                BuildActivitiesFilter();
            }
        }

        private void BuildUserPermissionsFilter()
        {
            var departmentIds = EntityFilter.UserPermissionsFilter.DepartmentIds;

            EntityPredicates.Add(x => departmentIds.Contains(x.DepartmentId));
        }

        private void BuildSpecificObjectivesFilter()
        {
            var specificObjectiveIds = EntityFilter.SpecificObjectivesFilter.SpecificObjectiveIds;

            EntityPredicates.Add(x => specificObjectiveIds.Contains(x.SpecificObjectiveId));
        }

        private void BuildActivitiesFilter()
        {
            var activityIds = EntityFilter.ActivitiesFilter.ActivityIds;

            EntityPredicates.Add(x => activityIds.Contains(x.Id));
        }

        private void BuildFunctionariesFilter()
        {
            var functionaryIds = EntityFilter.FunctionariesFilter.FunctionaryIds;

            EntityPredicates.Add(x => x.ActivityFunctionaries.Any(y => functionaryIds.Contains(y.FunctionaryId)));
        }
    }
}