using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Procedures;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Procedures
{
    internal class ProcedureHistoriesFilterComponent : DataExpressionFilterComponent<ProcedureHistory, ProcedureHistoriesFilterComponentContext>
    {
        public ProcedureHistoriesFilterComponent(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override Task<DataExpressions<ProcedureHistory>> SetCustomDataExpressionsAsync(ProcedureHistoriesFilterComponentContext context, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<ProcedureHistory>();

            if (!context.ProcedureHistoryFilter.IsEmpty())
            {
                var filterBuilder = new ProcedureHistoryFilterBuilder(ServiceProvider, context.ProcedureHistoryFilter);
                dataExpressions.AddRange(filterBuilder.Build());
            }

            return Task.FromResult(dataExpressions);
        }
    }
}
