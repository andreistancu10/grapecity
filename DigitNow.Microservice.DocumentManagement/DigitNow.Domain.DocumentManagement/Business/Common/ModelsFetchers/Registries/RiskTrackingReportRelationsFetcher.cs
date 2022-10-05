using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal class RiskTrackingReportRelationsFetcher : BaseRelationsFetcher
    {
        public IReadOnlyList<UserModel> RiskTrackingReportUsers
                => GetItems<RiskTrackingReportUsersFetcher, UserModel>();
        public RiskTrackingReportRelationsFetcher(IServiceProvider serviceProvider) : base(serviceProvider)
        { }

        public RiskTrackingReportRelationsFetcher UseRiskTrackingReportsContext(RiskTrackingReportFetcherContext context)
        {
            Aggregator
               .UseRemoteFetcher<RiskTrackingReportUsersFetcher>(context);

            return this;
        }
    }
}
