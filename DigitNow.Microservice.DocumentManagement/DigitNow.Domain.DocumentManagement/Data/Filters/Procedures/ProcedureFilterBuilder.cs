using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Procedures
{
    internal class ProcedureFilterBuilder : DataExpressionFilterBuilder<Procedure, ProcedureFilter>
    {
        public ProcedureFilterBuilder(IServiceProvider serviceProvider, ProcedureFilter filter): base(serviceProvider, filter) { }


        private void BuildFilterByProceduresGeneralObjectives()
        {
            if(EntityFilter.GeneralObjectivesFilter != null)
            {
                EntityPredicates.Add(x => EntityFilter.GeneralObjectivesFilter.GeneralObjectiveIds.Contains(x.GeneralObjectiveId));
            }
        }
        private void BuildFilterByProceduresSpecificObjectives()
        {
            if (EntityFilter.SpecificObjectivesFilter != null)
            {
                EntityPredicates.Add(x => EntityFilter.SpecificObjectivesFilter.SpecificObjectiveIds.Contains(x.SpecificObjectiveId));
            }
        }
        private void BuildFilterByProceduresActivities()
        {
            if (EntityFilter.ActivitiesFilter != null)
            {
                EntityPredicates.Add(x => EntityFilter.ActivitiesFilter.ActivityIds.Contains(x.ActivityId));
            }
        }
        private void BuildFilterByProceduresName()
        {
            if (EntityFilter.ProcedureNameFilter != null)
            {
                EntityPredicates.Add(x => x.Title.Equals(EntityFilter.ProcedureNameFilter.ProcedureName));
            }
        }
        private void BuildFilterByProceduresState()
        {
            if (EntityFilter.ProcedureStateFilter != null)
            {
                EntityPredicates.Add(x => x.State == EntityFilter.ProcedureStateFilter.ProcedureState);
            }
        }
        private void BuildFilterByProceduresCategories()
        {
            if (EntityFilter.ProcedureCategoriesFilter != null)
            {
                EntityPredicates.Add(x => EntityFilter.ProcedureCategoriesFilter.ProcedureCategories.Contains(x.ProcedureCategory));
            }
        }
        private void BuildFilterByProceduresDepartments()
        {
            if (EntityFilter.DepartmentsFilter != null)
            {
                EntityPredicates.Add(x => EntityFilter.DepartmentsFilter.DepartmentIds.Contains(x.DepartmentId));
            }
        }
        private void BuildFilterByProceduresStartDate()
        {
            if (EntityFilter.StartDateFilter != null)
            {
                EntityPredicates.Add(x => x.StartDate.Date == EntityFilter.StartDateFilter.StartDate.Date);
            }
        }

        protected override void InternalBuild()
        {
            BuildFilterByProceduresGeneralObjectives();
            BuildFilterByProceduresSpecificObjectives();
            BuildFilterByProceduresActivities();
            BuildFilterByProceduresName();
            BuildFilterByProceduresState();
            BuildFilterByProceduresCategories();
            BuildFilterByProceduresDepartments();
            BuildFilterByProceduresStartDate();
        }
    }
}
