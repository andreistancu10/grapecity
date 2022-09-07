using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Objectives
{
    internal class GeneralObjectiveFilterBuilder : DataExpressionFilterBuilder<GeneralObjective, GeneralObjectiveFilter>
    {
        public GeneralObjectiveFilterBuilder(IServiceProvider serviceProvider, GeneralObjectiveFilter filter)
           : base(serviceProvider, filter) { }

        private void BuildFilterByCreationDate()
        {
            var creationDateFilter = EntityFilter.GeneralObjectiveRegistrationDateFilter;
            if (creationDateFilter != null)
            {
                EntityPredicates.Add(objective =>
                    objective.CreatedAt.Date == creationDateFilter.CreationDate.Date
                );
            }
        }

        private void BuildFilterByTitle()
        {
            var titleFilter = EntityFilter.GeneralObjectiveTitleFilter;
            if (titleFilter != null)
            {
                EntityPredicates.Add(objective =>
                    objective.Objective.Title.Equals(titleFilter.Title)
                );
            }
        }
        private void BuildFilterByCode()
        {
            var codeFilter = EntityFilter.GeneralObjectiveCodeFilter;
            if (codeFilter != null)
            {
                EntityPredicates.Add(objective =>
                    objective.Objective.Code.Equals(codeFilter.Code)
                );
            }
        }
        private void BuildFilterByState()
        {
            var stateFilter = EntityFilter.GeneralObjectiveStateFilter;
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
