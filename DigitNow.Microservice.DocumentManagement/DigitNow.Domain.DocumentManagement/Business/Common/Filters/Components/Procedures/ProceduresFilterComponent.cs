using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Procedures;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Procedures
{
    internal class ProceduresFilterComponent : DataExpressionFilterComponent<Procedure, ProceduresFilterComponentContext>
    {
        public ProceduresFilterComponent(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override Task<DataExpressions<Procedure>> SetCustomDataExpressionsAsync(ProceduresFilterComponentContext context, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<Procedure>();
            if (!context.ProcedureFilter.IsEmpty())
            {
                var filterBuilder = new ProcedureFilterBuilder(ServiceProvider, context.ProcedureFilter);
                dataExpressions.AddRange(filterBuilder.Build());
            }

            return Task.FromResult(dataExpressions);
        }
    }
}
