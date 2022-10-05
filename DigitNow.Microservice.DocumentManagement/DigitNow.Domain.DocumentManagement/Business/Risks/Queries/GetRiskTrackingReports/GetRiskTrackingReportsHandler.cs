using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Risks.Queries.GetRiskTrackingReports
{
    public class GetRiskTrackingReportsHandler : IQueryHandler<GetRiskTrackingReportsQuery, ResultObject>
    {
        private readonly IRiskTrackingReportService _riskTrackingReportService;
        private readonly IRiskTrackingReportMappingService _riskTrackingReportMappingService;

        public GetRiskTrackingReportsHandler(IRiskTrackingReportService riskTrackingReportService, IRiskTrackingReportMappingService riskTrackingReportMappingService)
        {
            _riskTrackingReportService = riskTrackingReportService;
            _riskTrackingReportMappingService = riskTrackingReportMappingService;
        }

        public async Task<ResultObject> Handle(GetRiskTrackingReportsQuery query, CancellationToken cancellationToken)
        {
            var totalItems = await _riskTrackingReportService.CountAsync(query.RiskId, cancellationToken);

            if (totalItems == 0) return ResultObject.Ok(GetEmptyPageRiskTrackingReportsResponse(query));

            var riskTrackingReports = await _riskTrackingReportService.GetAllAsync(query.RiskId,
                query.Page,
                query.Count,
                cancellationToken);

            var viewModel = await _riskTrackingReportMappingService.MapToRiskTrackingViewModelAsync(riskTrackingReports, cancellationToken);

            return ResultObject.Ok(BuildFirstPageRiskTrackingReportsResponse(query, totalItems, viewModel));
        }

        private static GetRiskTrackingReportsResponse BuildFirstPageRiskTrackingReportsResponse(GetRiskTrackingReportsQuery query, long totalItems, List<RiskTrackingReportViewModel> items)
        {
            var pageCount = totalItems / query.Count;

            if (items.Count % query.Count > 0)
            {
                pageCount += 1;
            }

            return new GetRiskTrackingReportsResponse
            {
                TotalItems = totalItems,
                TotalPages = pageCount,
                PageNumber = query.Page,
                PageSize = query.Count,
                Items = items.OrderByDescending(x => x.CreatedAt).ToList()
            };
        }

        private static GetRiskTrackingReportsResponse GetEmptyPageRiskTrackingReportsResponse(GetRiskTrackingReportsQuery query)
        {
            return new GetRiskTrackingReportsResponse
            {
                TotalItems = 0,
                TotalPages = 1,
                PageNumber = query.Page,
                PageSize = query.Count,
                Items = new List<RiskTrackingReportViewModel>()
            };
        }
    }
}
