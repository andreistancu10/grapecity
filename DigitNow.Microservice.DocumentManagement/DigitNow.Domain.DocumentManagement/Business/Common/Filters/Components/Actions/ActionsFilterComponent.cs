using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Filters;
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
                UserPermissionsFilter = new ActionUserPermissionsFilter
                {
                    DepartmentIds = SetDepartmentsFilter(context.ActionsFilter.DepartmentsFilter.DepartmentIds, currentUser)
                },
                ActionsFilter = new ActionActionsFilter
                {
                    ActionIds = context.ActionsFilter.ActionsFilter.ActionIds,
                },
                FunctionariesFilter = new ActionFunctionariesFilter
                {
                    FunctionaryIds = context.ActionsFilter.FunctionariesFilter.FunctionaryIds
                },
                SpecificObjectivesFilter = new ActionSpecificObjectivesFilter
                {
                    SpecificObjectiveIds = context.ActionsFilter.SpecificObjectivesFilter.SpecificObjectiveIds
                }
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