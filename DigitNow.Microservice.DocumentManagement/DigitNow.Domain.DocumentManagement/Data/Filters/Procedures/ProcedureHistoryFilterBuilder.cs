using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Procedures
{
    internal class ProcedureHistoryFilterBuilder : DataExpressionFilterBuilder<ProcedureHistory, ProcedureHistoryFilter>
    {
        public ProcedureHistoryFilterBuilder(IServiceProvider serviceProvider, ProcedureHistoryFilter filter) : base(serviceProvider,
            filter)
        {
        }

        private void BuildFilterByProceduresStartDate()
        {
            if (EntityFilter.ProcedureHistoryProceduresFilter != null)
            {
                EntityPredicates.Add(x => EntityFilter.ProcedureHistoryProceduresFilter.ProcedureIds.Contains(x.ProcedureId));
            }
        }

        protected override void InternalBuild()
        {
            BuildFilterByProceduresStartDate();
        }
    }
}