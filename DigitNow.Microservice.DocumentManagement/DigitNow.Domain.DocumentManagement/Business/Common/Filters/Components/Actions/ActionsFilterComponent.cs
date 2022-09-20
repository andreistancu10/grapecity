using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Common;
using Action = DigitNow.Domain.DocumentManagement.Data.Entities.Action;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Actions
{
    internal class ActionsFilterComponent : DataExpressionFilterComponent<Action, ActionsFilterComponentContext>
    {
        public ActionsFilterComponent(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Task<DataExpressions<Action>> SetCustomDataExpressionsAsync(ActionsFilterComponentContext context, CancellationToken token)
        {
            var currentUser = context.CurrentUser;

            var filter = new ActionPermissionsFilter
            {
                DepartmentsFilter = new DepartmentsFilter
                {
                    DepartmentIds = SetDepartmentsFilter(context.ActionFilter.DepartmentsFilter.DepartmentIds, currentUser)
                },
                ActionsFilter = context.ActionFilter.ActionsFilter,
                FunctionariesFilter = context.ActionFilter.FunctionariesFilter,
                SpecificObjectivesFilter = context.ActionFilter.SpecificObjectivesFilter,
                ActivitiesFilter = context.ActionFilter.ActivitiesFilter
            };

            return Task.FromResult(new ActionFilterBuilder(ServiceProvider, filter)
                .Build());
        }

        private static List<long> SetDepartmentsFilter(IReadOnlyCollection<long> departmentsFilterDepartmentIds, UserModel currentUser)
        {
            departmentsFilterDepartmentIds = departmentsFilterDepartmentIds.Any()
                ? currentUser.Departments.Select(c => c.Id).Intersect(departmentsFilterDepartmentIds).ToList()
                : currentUser.Departments.Select(c => c.Id).ToList();

            if (!departmentsFilterDepartmentIds.Any())
            {
                departmentsFilterDepartmentIds = currentUser.Departments.Select(c => c.Id).ToList();
            }

            return departmentsFilterDepartmentIds.ToList();
        }
    }
}