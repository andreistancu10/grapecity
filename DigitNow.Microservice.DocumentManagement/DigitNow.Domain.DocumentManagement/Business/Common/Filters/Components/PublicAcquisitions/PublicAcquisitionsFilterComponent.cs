using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.PublicAcquisitions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.PublicAcquisitions
{
    internal class PublicAcquisitionsFilterComponent : DataExpressionFilterComponent<PublicAcquisitionProject, PublicAcquisitionsFilterComponentContext>
    {
        public PublicAcquisitionsFilterComponent(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override Task<DataExpressions<PublicAcquisitionProject>> SetCustomDataExpressionsAsync(PublicAcquisitionsFilterComponentContext context, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<PublicAcquisitionProject>();
            if (!context.PublicAcquisitionFilter.IsEmpty())
            {
                var filterBuilder = new PublicAcquisitionFilterBuilder(ServiceProvider, context.PublicAcquisitionFilter);
                dataExpressions.AddRange(filterBuilder.Build());
            }

            return Task.FromResult(dataExpressions);
        }
    }
}
