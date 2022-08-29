using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Objectives
{
    internal class ObjectiveFilterBuilder : DataExpressionFilterBuilder<GeneralObjective, ObjectiveFilter>
    {
        public ObjectiveFilterBuilder(IServiceProvider serviceProvider, ObjectiveFilter filter)
           : base(serviceProvider, filter) { }

        private void BuildFilterByCreationDate()
        {
            var creationDateFilter = EntityFilter.ObjectiveRegistrationDateFilter;
            if (creationDateFilter != null)
            {
                EntityPredicates.Add(objective =>
                    objective.CreatedAt.Date == creationDateFilter.CreationDate.Date
                );
            }
        }

        private void BuildFilterByTitle()
        {
            var titleFilter = EntityFilter.ObjectiveTitleFilter;
            if (titleFilter != null)
            {
                EntityPredicates.Add(objective =>
                    objective.Objective.Title.Equals(titleFilter.Title)
                );
            }
        }
        private void BuildFilterByCode()
        {
            var codeFilter = EntityFilter.ObjectiveCodeFilter;
            if (codeFilter != null)
            {
                EntityPredicates.Add(objective =>
                    objective.Objective.Code.Equals(codeFilter.Code)
                );
            }
        }
        private void BuildFilterByState()
        {
            var stateFilter = EntityFilter.ObjectiveStateFilter;
            if (stateFilter != null)
            {
                EntityPredicates.Add(objective =>
                    objective.Objective.State == stateFilter.State
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
