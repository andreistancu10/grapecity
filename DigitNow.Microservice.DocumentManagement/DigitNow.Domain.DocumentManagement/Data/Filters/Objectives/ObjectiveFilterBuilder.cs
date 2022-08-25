using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Objectives
{
    internal class ObjectiveFilterBuilder : DataExpressionFilterBuilder<Objective, ObjectiveFilter>
    {
        public ObjectiveFilterBuilder(IServiceProvider serviceProvider, ObjectiveFilter filter)
           : base(serviceProvider, filter) { }

        private void BuildFilterByCreationDate()
        {
            var creationDateFilter = EntityFilter.ObjectiveCreationDateFilter;
            if (creationDateFilter != null)
            {
                EntityPredicates.Add(objective =>
                    objective.CreatedAt.Date == creationDateFilter.CreationDate
                );
            }
        }

        private void BuildFilterByTitle()
        {
            var titleFilter = EntityFilter.ObjectiveTitleFilter;
            if (titleFilter != null)
            {
                EntityPredicates.Add(objective =>
                    objective.Title.Equals(titleFilter.Title)
                );
            }
        }
        private void BuildFilterByCode()
        {
            var codeFilter = EntityFilter.ObjectiveCodeFilter;
            if (codeFilter != null)
            {
                EntityPredicates.Add(objective =>
                    objective.Code.Equals(codeFilter.Code)
                );
            }
        }
        private void BuildFilterByState()
        {
            var stateFilter = EntityFilter.ObjectiveStateFilter;
            if (stateFilter != null)
            {
                EntityPredicates.Add(objective =>
                    objective.State == stateFilter.State
                );
            }
        }

        protected override void InternalBuild()
        {
            BuildFilterByCreationDate();
            BuildFilterByTitle();
            BuildFilterByCode();
            BuildFilterByState();
        }
    }
}
