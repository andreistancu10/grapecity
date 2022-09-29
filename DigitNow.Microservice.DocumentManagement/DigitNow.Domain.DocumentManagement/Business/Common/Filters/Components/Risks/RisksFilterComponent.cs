using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Risks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Risks
{
    internal class RisksFilterComponent : DataExpressionFilterComponent<Risk, RisksFilterComponentContext>
    {
        public RisksFilterComponent(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override Task<DataExpressions<Risk>> SetCustomDataExpressionsAsync(RisksFilterComponentContext context, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<Risk>();
            if (!context.RiskFilter.IsEmpty())
            {
                var filterBuilder = new RiskFilterBuilder(ServiceProvider, context.RiskFilter);
                dataExpressions.AddRange(filterBuilder.Build());
            }

            return Task.FromResult(dataExpressions);
        }
    }
}
