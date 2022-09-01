using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.SpecificObjectivesPermissions
{
    internal class SpecificObjectivePermissionsFilterBuilder : DataExpressionFilterBuilder<SpecificObjective, SpecificObjectivePermissionsFilter>
    {
        public SpecificObjectivePermissionsFilterBuilder(IServiceProvider serviceProvider, SpecificObjectivePermissionsFilter filter)
            : base(serviceProvider, filter)
        {
        }

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
            var usreId = EntityFilter.UserPermissionsFilter.UserId;

            EntityPredicates.Add(x => x.CreatedBy == usreId || departments.Contains(x.DepartmentId));
        }
    }
}
