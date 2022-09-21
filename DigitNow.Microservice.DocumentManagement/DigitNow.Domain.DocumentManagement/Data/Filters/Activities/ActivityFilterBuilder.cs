using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Activities
{
    internal class ActivityFilterBuilder : DataExpressionFilterBuilder<Activity, ActivityFilter>
    {
        public ActivityFilterBuilder(IServiceProvider serviceProvider, ActivityFilter filter)
            : base(serviceProvider, filter)
        {
        }

        private void BuildSpecificObjectivesFilter()
        {
            if (EntityFilter.SpecificObjectivesFilter != null)
            {
                var specificObjectiveIds = EntityFilter.SpecificObjectivesFilter.SpecificObjectiveIds;
                EntityPredicates.Add(x => specificObjectiveIds.Contains(x.SpecificObjectiveId)); 
            }
        }

        private void BuildActivitiesFilter()
        {
            if (EntityFilter.ActivitiesFilter != null)
            {
                var activityIds = EntityFilter.ActivitiesFilter.ActivityIds;
                EntityPredicates.Add(x => activityIds.Contains(x.Id)); 
            }
        }

        private void BuildFunctionariesFilter()
        {
            if (EntityFilter.FunctionariesFilter != null)
            {
                var functionaryIds = EntityFilter.FunctionariesFilter.FunctionaryIds;
                EntityPredicates.Add(x => x.ActivityFunctionaries.Any(y => functionaryIds.Contains(y.FunctionaryId))); 
            }
        }

        protected override void InternalBuild()
        {
            BuildSpecificObjectivesFilter();
            BuildFunctionariesFilter();
            BuildActivitiesFilter();
        }
    }
}