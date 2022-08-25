using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Objectives
{
    internal class ObjectivesFilterComponent : DataExpressionFilterComponent<Objective, ObjectivesFilterComponentContext>
    {
        #region [ Construction ]
        public ObjectivesFilterComponent(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        #endregion

        protected override Task<DataExpressions<Objective>> SetCustomDataExpressionsAsync(ObjectivesFilterComponentContext context, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<Objective>();

            if (!context.ObjectiveFilter.IsEmpty())
            {
                var filterBuilder = new ObjectiveFilterBuilder(ServiceProvider, context.ObjectiveFilter);
                dataExpressions.AddRange(filterBuilder.Build());
            }

            return Task.FromResult(dataExpressions);
        }
    }
}
