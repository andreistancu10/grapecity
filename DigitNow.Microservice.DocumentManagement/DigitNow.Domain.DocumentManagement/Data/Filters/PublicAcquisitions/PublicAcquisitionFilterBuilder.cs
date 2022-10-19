using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.PublicAcquisitions
{
    internal class PublicAcquisitionFilterBuilder : DataExpressionFilterBuilder<PublicAcquisitionProject, PublicAcquisitionFilter>
    {
        public PublicAcquisitionFilterBuilder(IServiceProvider serviceProvider, PublicAcquisitionFilter filter)
        : base(serviceProvider, filter) { }

        private void BuildFilterByProjectYear()
        {
            if (EntityFilter.ProjectYearFilter != null)
            {
                EntityPredicates.Add(x => x.ProjectYear == EntityFilter.ProjectYearFilter.ProjectYear);
            }
        }

        private void BuildFilterByType()
        {
            if (EntityFilter.TypeFilter != null)
            {
                EntityPredicates.Add(x => x.Type.Equals(EntityFilter.TypeFilter.Type));
            }
        }

        private void BuildFilterByCpvCode()
        {
            if (EntityFilter.CpvCodeFilter != null)
            {
                EntityPredicates.Add(x => x.CpvCode == EntityFilter.CpvCodeFilter.CpvCode);
            }
        }

        private void BuildFilterByFinancingSource()
        {
            if (EntityFilter.FinancingSourceFilter != null)
            {
                EntityPredicates.Add(x => x.FinancingSource == EntityFilter.FinancingSourceFilter.FinancingSource);
            }
        }

        private void BuildFilterByEstablishedProcedure()
        {
            if (EntityFilter.EstablishedProcedureFilter != null)
            {
                EntityPredicates.Add(x => x.EstablishedProcedure == EntityFilter.EstablishedProcedureFilter.EstablishedProcedure);
            }
        }

        private void BuildFilterByEstimatedMonthForInitiatingProcedure()
        {
            if (EntityFilter.EstimatedMonthForInitiatingProcedureFilter != null)
            {
                EntityPredicates.Add(x => x.EstimatedMonthForInitiatingProcedure.Equals(EntityFilter.EstimatedMonthForInitiatingProcedureFilter.EstimatedMonthForInitiatingProcedure));
            }
        }

        private void BuildFilterByEstimatedMonthForProcedureAssignment()
        {
            if (EntityFilter.EstimatedMonthForProcedureAssignmentFilter != null)
            {
                EntityPredicates.Add(x => x.EstimatedMonthForProcedureAssignment.Equals(EntityFilter.EstimatedMonthForProcedureAssignmentFilter.EstimatedMonthForProcedureAssignment));
            }
        }

        private void BuildFilterByProcedureAssignmentMethod()
        {
            if (EntityFilter.ProcedureAssignmentMethodFilter != null)
            {
                EntityPredicates.Add(x => x.ProcedureAssignmentMethod == EntityFilter.ProcedureAssignmentMethodFilter.ProcedureAssignmentMethod);
            }
        }

        protected override void InternalBuild()
        {
            BuildFilterByProcedureAssignmentMethod();
            BuildFilterByEstimatedMonthForProcedureAssignment();
            BuildFilterByEstimatedMonthForInitiatingProcedure();
            BuildFilterByEstablishedProcedure();
            BuildFilterByFinancingSource();
            BuildFilterByCpvCode();
            BuildFilterByType();
            BuildFilterByProjectYear();
        }
    }
}
