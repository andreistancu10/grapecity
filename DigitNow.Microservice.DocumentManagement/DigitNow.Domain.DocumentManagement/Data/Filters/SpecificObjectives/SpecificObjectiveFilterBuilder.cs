using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.SpecificObjectives
{
    internal class SpecificObjectiveFilterBuilder : DataExpressionFilterBuilder<SpecificObjective, SpecificObjectiveFilter>
    {
        public SpecificObjectiveFilterBuilder(IServiceProvider serviceProvider, SpecificObjectiveFilter filter)
            : base(serviceProvider, filter)
        {
        }

        protected override void InternalBuild()
        {
            BuildGeneralObjectiveIdFilter();

            if (EntityFilter.CodeFilter != null)
            {
                BuildCodeFilter();
            }
            else
            {
                BuildTitleFilter();
                BuildDepartmentFilter();
                BuildFunctionaryFilter();
                BuildStateFilter();
            }
        }

        private void BuildGeneralObjectiveIdFilter()
        {
            if(EntityFilter.GeneralObjectiveIdFilter != null)
                EntityPredicates.Add(x => x.GeneralObjectiveId == EntityFilter.GeneralObjectiveIdFilter.ObjectiveId);
        }

        private void BuildCodeFilter()
        {
            EntityPredicates.Add(x => x.Objective.Code == EntityFilter.CodeFilter.Code);
        }

        private void BuildTitleFilter()
        {
            if (EntityFilter.TitleFilter != null)
            {
                EntityPredicates.Add(x => x.Objective.Title == EntityFilter.TitleFilter.Title);
            }
        }

        private void BuildDepartmentFilter()
        {
            if (EntityFilter.DepartmentFilter != null)
            {
                EntityPredicates.Add(x => x.DepartmentId == EntityFilter.DepartmentFilter.DepartmentId);
            }
        }

        private void BuildFunctionaryFilter()
        {
            if (EntityFilter.FunctionaryFilter != null)
            {
                EntityPredicates.Add(x => x.SpecificObjectiveFunctionarys.Any(x => x.FunctionaryId == EntityFilter.FunctionaryFilter.FunctionaryId));
            }
        }

        private void BuildStateFilter()
        {
            if (EntityFilter.StateFilter != null)
            {
                EntityPredicates.Add(x => x.Objective.State == EntityFilter.StateFilter.StateId);
            }
        }
    }
}
