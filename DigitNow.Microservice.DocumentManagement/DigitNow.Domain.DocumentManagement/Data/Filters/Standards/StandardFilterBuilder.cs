using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Standards
{
    internal class StandardFilterBuilder : DataExpressionFilterBuilder<Standard, StandardFilter>
    {
        public StandardFilterBuilder(IServiceProvider serviceProvider, StandardFilter filter)
                : base(serviceProvider, filter) { }

        private void BuildFilterByStandardsTitle()
        {
            if(EntityFilter.TitleFilter != null)
            {
                EntityPredicates.Add(x => x.Title.Equals(EntityFilter.TitleFilter.Title));
            }
        }
        private void BuildFilterByStandardsDepartment()
        {
            if (EntityFilter.DepartmentFilter != null)
            {
                EntityPredicates.Add(x => x.DepartmentId == EntityFilter.DepartmentFilter.DepartmentId);
            }
        }
        private void BuildFilterByStandardsFunctionaries()
        {
            if (EntityFilter.FunctionariesFilter != null)
            {
                var functionaryIds = EntityFilter.FunctionariesFilter.FunctionariesIds;
                EntityPredicates.Add(x => x.StandardFunctionaries.Any(y => functionaryIds.Contains(y.FunctionaryId)));
            }
        }
        private void BuildFilterByStandardsDeadline()
        {
            if (EntityFilter.DeadlineFilter != null)
            {
                EntityPredicates.Add(x => x.Deadline.Date == EntityFilter.DeadlineFilter.Deadline.Date);
            }
        }
        protected override void InternalBuild()
        {
            BuildFilterByStandardsTitle();
            BuildFilterByStandardsDepartment();
            BuildFilterByStandardsFunctionaries();
            BuildFilterByStandardsDeadline();
        }
    }
}
