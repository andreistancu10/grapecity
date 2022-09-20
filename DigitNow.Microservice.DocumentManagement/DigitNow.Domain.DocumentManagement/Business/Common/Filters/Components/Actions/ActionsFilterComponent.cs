using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Actions;
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
                DepartmentsFilter = new ActionUserPermissionsFilter
                {
                    DepartmentIds = SetDepartmentsFilter(context.ActionFilter.DepartmentsFilter.DepartmentIds, currentUser)
                },
                ActionsFilter = new ActionsFilter
                {
                    ActionIds = context.ActionFilter.ActionsFilter.ActionIds,
                },
                FunctionariesFilter = new ActionFunctionariesFilter
                {
                    FunctionaryIds = context.ActionFilter.FunctionariesFilter.FunctionaryIds
                },
                SpecificObjectivesFilter = new ActionSpecificObjectivesFilter
                {
                    SpecificObjectiveIds = context.ActionFilter.SpecificObjectivesFilter.SpecificObjectiveIds
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