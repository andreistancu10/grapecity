using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.SpecificObjectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Objectives
{
    internal class SpecificObjectivesFilterComponenet : DataExpressionFilterComponent<SpecificObjective, SpecificObjectivesFilterComponenetContext>
    {
        public SpecificObjectivesFilterComponenet(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        protected override Task<DataExpressions<SpecificObjective>> SetBuiltinDataExpressionsAsync(SpecificObjectivesFilterComponenetContext context, CancellationToken token)
        {
            return Task.FromResult(new DataExpressions<SpecificObjective>
            {
                x => x.CreatedAt.Year ==  DateTime.UtcNow.Year
            });
        }

        protected override Task<DataExpressions<SpecificObjective>> SetCustomDataExpressionsAsync(SpecificObjectivesFilterComponenetContext context, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<SpecificObjective>();

            if (!context.SpecificObjectiveFilter.IsEmpty())
            {
                var filterBuilder = new SpecificObjectiveFilterBuilder(ServiceProvider, context.SpecificObjectiveFilter);
                dataExpressions.AddRange(filterBuilder.Build());
            }

            return Task.FromResult(dataExpressions);
        }
    }
}
