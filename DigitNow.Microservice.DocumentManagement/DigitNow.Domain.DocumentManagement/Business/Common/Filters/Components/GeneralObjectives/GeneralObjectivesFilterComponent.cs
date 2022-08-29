using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.GeneralObjectives
{
    internal class GeneralObjectivesFilterComponent : DataExpressionFilterComponent<GeneralObjective, GeneralObjectivesFilterComponentContext>
    {
        #region [ Construction ]
        public GeneralObjectivesFilterComponent(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        #endregion

        protected override Task<DataExpressions<GeneralObjective>> SetCustomDataExpressionsAsync(GeneralObjectivesFilterComponentContext context, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<GeneralObjective>();

            if (!context.GeneralObjectiveFilter.IsEmpty())
            {
                var filterBuilder = new GeneralObjectiveFilterBuilder(ServiceProvider, context.GeneralObjectiveFilter);
                dataExpressions.AddRange(filterBuilder.Build());
            }

            return Task.FromResult(dataExpressions);
        }
    }
}
