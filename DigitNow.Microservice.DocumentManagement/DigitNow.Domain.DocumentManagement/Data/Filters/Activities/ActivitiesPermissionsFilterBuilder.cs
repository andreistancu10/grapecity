using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Activities
{
    internal class ActivitiesPermissionsFilterBuilder : DataExpressionFilterBuilder<Activity, ActivitiesPermissionsFilter>
    {
        public ActivitiesPermissionsFilterBuilder(IServiceProvider serviceProvider, ActivitiesPermissionsFilter filter)
            : base(serviceProvider, filter) { }

        protected override void InternalBuild()
        {
            if (EntityFilter.UserPermissionsFilter != null)
            {
                BuildUserPermissionsFilter();
            }
        }
        private void BuildUserPermissionsFilter()
        {
            var departments = EntityFilter.UserPermissionsFilter.DepartmentIds;

            EntityPredicates.Add(x => departments.Contains(x.DepartmentId));
        }
    }
}
