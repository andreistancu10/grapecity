using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts
{
    internal class RiskTrackingReportFetcherContext : ModelFetcherContext
    {
        public IList<RiskTrackingReport> RiskTrackingReports
        {
            get => this[nameof(RiskTrackingReport)] as IList<RiskTrackingReport>;
            set => this[nameof(RiskTrackingReport)] = value;
        }
    }
}
