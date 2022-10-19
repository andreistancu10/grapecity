using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Standards;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Standards
{
    internal class StandardsFilterComponent : DataExpressionFilterComponent<Standard, StandardsFilterComponentContext>
    {
        public StandardsFilterComponent(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override Task<DataExpressions<Standard>> SetCustomDataExpressionsAsync(StandardsFilterComponentContext context, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<Standard>();
            if (!context.StandardFilter.IsEmpty())
            {
                var filterBuilder = new StandardFilterBuilder(ServiceProvider, context.StandardFilter);
                dataExpressions.AddRange(filterBuilder.Build());
            }

            return Task.FromResult(dataExpressions);
        }
    }
}
