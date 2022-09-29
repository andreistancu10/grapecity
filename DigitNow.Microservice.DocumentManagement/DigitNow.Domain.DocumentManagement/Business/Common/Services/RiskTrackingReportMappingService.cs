using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IRiskTrackingReportMappingService
    {
        Task<List<RiskTrackingReportViewModel>> MapToRiskTrackingViewModelAsync(IList<RiskTrackingReport> riskTrackingReports, CancellationToken cancellationToken);
    }
    public class RiskTrackingReportMappingService : IRiskTrackingReportMappingService
    {
        private readonly IMapper _mapper;
        private readonly RiskTrackingReportRelationsFetcher _riskTrackingReportRelationsFetcher;

        public RiskTrackingReportMappingService(IMapper mapper, IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _riskTrackingReportRelationsFetcher = new RiskTrackingReportRelationsFetcher(serviceProvider);
        }
        public async Task<List<RiskTrackingReportViewModel>> MapToRiskTrackingViewModelAsync(IList<RiskTrackingReport> riskTrackingReports, CancellationToken cancellationToken)
        {
            await _riskTrackingReportRelationsFetcher
                .UseRiskTrackingReportsContext(new RiskTrackingReportFetcherContext { RiskTrackingReports = riskTrackingReports })
                .TriggerFetchersAsync(cancellationToken);

            return MapRiskTrackingReports(riskTrackingReports)
                .ToList();
        }

        private List<RiskTrackingReportViewModel> MapRiskTrackingReports(IEnumerable<RiskTrackingReport> riskTrackingReports)
        {
            var result = new List<RiskTrackingReportViewModel>();

            foreach (var riskTrackingReport in riskTrackingReports)
            {
                var aggregate = new RiskTrackingReportAggregate
                {
                    RiskTrackingReport = riskTrackingReport,
                    Users = _riskTrackingReportRelationsFetcher.RiskTrackingReportUsers,
                };

                result.Add(_mapper.Map<RiskTrackingReportAggregate, RiskTrackingReportViewModel>(aggregate));
            }

            return result;
        }
    }
}
