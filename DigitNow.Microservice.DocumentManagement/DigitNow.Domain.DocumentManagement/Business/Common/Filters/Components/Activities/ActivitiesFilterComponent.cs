using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Activities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Activities
{
    internal class ActivitiesFilterComponent : DataExpressionFilterComponent<Activity, ActivitiesFilterComponentContext>
    {
        public ActivitiesFilterComponent(IServiceProvider serviceProvider) : base(serviceProvider){}

        protected override Task<DataExpressions<Activity>> SetCustomDataExpressionsAsync(ActivitiesFilterComponentContext context, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<Activity>();

            if (!context.ActivityFilter.IsEmpty())
            {
                var filterBuilder = new ActivityFilterBuilder(ServiceProvider, context.ActivityFilter);
                dataExpressions.AddRange(filterBuilder.Build());
            }

            return Task.FromResult(dataExpressions);
        }
    }
}