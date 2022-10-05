using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Risks
{
    internal class RiskFilterBuilder : DataExpressionFilterBuilder<Risk, RiskFilter>
    {
        public RiskFilterBuilder(IServiceProvider serviceProvider, RiskFilter filter)
                : base(serviceProvider, filter) { }


        private void BuildFilterByRisksGeneralObjective()
        {
            if (EntityFilter.GeneralObjectiveFilter != null)
            {
                EntityPredicates.Add(x => x.GeneralObjectiveId == EntityFilter.GeneralObjectiveFilter.GeneralObjectiveId);
            }
        }

        private void BuildFilterByRisksSpecificObjective()
        {
            if (EntityFilter.SpecificObjectiveFilter != null)
            {
                EntityPredicates.Add(x => x.SpecificObjectiveId == EntityFilter.SpecificObjectiveFilter.SpecificObjectiveId);
            }
        }

        private void BuildFilterByRisksActivity()
        {
            if (EntityFilter.ActivityFilter != null)
            {
                EntityPredicates.Add(x => x.ActivityId == EntityFilter.ActivityFilter.ActivityId);
            }
        }

        private void BuildFilterByRisksName()
        {
            if (EntityFilter.RiskNameFilter != null)
            {
                EntityPredicates.Add(x => x.Description == EntityFilter.RiskNameFilter.RiskName);
            }
        }

        private void BuildFilterByRisksDepartment()
        {
            if (EntityFilter.DepartmentFilter != null)
            {
                EntityPredicates.Add(x => x.DepartmentId == EntityFilter.DepartmentFilter.DepartmentId);
            }
        }

        private void BuildFilterByRisksLastRevision()
        {
            if (EntityFilter.LastRevisionFilter != null)
            {
                EntityPredicates.Add(x => x.ModifiedAt.HasValue ? 
                x.ModifiedAt.Value.Date == EntityFilter.LastRevisionFilter.DateOfLastRevision.Date :
                x.CreatedAt.Date == EntityFilter.LastRevisionFilter.DateOfLastRevision.Date);
            }
        }

        private void BuildFilterByRisksProbabilityOfApparition()
        {
            if (EntityFilter.ProbabilityOfApparitionFilter != null)
            {
                EntityPredicates.Add(x => x.ProbabilityOfApparitionEstimation == EntityFilter.ProbabilityOfApparitionFilter.ProbabilityOfApparitionEstimation);
            }
        }

        private void BuildFilterByRisksImpactOfObjectives()
        {
            if (EntityFilter.ImpactOfObjectivesEstimationFilter != null)
            {
                EntityPredicates.Add(x => x.ImpactOfObjectivesEstimation == EntityFilter.ImpactOfObjectivesEstimationFilter.ImpactOfObjectivesEstimation);
            }
        }

        protected override void InternalBuild()
        {
            BuildFilterByRisksGeneralObjective();
            BuildFilterByRisksSpecificObjective();
            BuildFilterByRisksActivity();
            BuildFilterByRisksName();
            BuildFilterByRisksDepartment();
            BuildFilterByRisksLastRevision();
            BuildFilterByRisksProbabilityOfApparition();
            BuildFilterByRisksImpactOfObjectives();
        }
    }
}
