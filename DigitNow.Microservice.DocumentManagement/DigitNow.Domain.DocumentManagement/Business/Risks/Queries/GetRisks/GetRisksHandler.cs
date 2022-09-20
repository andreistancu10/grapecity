using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Risks.Queries.GetRisks
{
    public class GetRisksHandler : IQueryHandler<GetRisksQuery, ResultObject>
    {
        private readonly IRiskService _riskService;
        private readonly IRiskMappingService _riskMappingService;

        public GetRisksHandler(IRiskService riskService, IRiskMappingService riskMappingService)
        {
            _riskService = riskService;
            _riskMappingService = riskMappingService;
        }
        public async Task<ResultObject> Handle(GetRisksQuery request, CancellationToken cancellationToken)
        {
            var totalItems = await _riskService.CountRisksAsync(request.Filter, cancellationToken);

            if (totalItems == 0) return ResultObject.Ok(GetEmptyPageRisksResponse(request));

            var risks = await _riskService.GetRisksAsync(request.Filter,
                request.Page,
                request.Count,
                cancellationToken);

            var viewModel = await _riskMappingService.MapToRiskViewModelAsync(risks, cancellationToken);

            return ResultObject.Ok(BuildFirstPageRisksResponse(request, totalItems, viewModel));
        }

        private static GetRisksResponse BuildFirstPageRisksResponse(GetRisksQuery query, long totalItems, List<RiskViewModel> items)
        {
            var pageCount = totalItems / query.Count;

            if (items.Count % query.Count > 0)
            {
                pageCount += 1;
            }

            return new GetRisksResponse
            {
                TotalItems = totalItems,
                TotalPages = pageCount,
                PageNumber = query.Page,
                PageSize = query.Count,
                Items = items
            };
        }

        private static GetRisksResponse GetEmptyPageRisksResponse(GetRisksQuery query)
        {
            return new GetRisksResponse
            {
                TotalItems = 0,
                TotalPages = 1,
                PageNumber = query.Page,
                PageSize = query.Count,
                Items = new List<RiskViewModel>()
            };
        }
    }
}
