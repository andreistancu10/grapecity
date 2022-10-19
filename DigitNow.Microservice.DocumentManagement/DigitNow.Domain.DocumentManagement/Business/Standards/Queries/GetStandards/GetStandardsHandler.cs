using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Standards.Queries.GetStandards
{
    public class GetStandardsHandler : IQueryHandler<GetStandardsQuery, ResultObject>
    {
        private readonly IStandardService _standardService;
        private readonly IStandardMappingService _standardMappingService;

        public GetStandardsHandler(IStandardService standardService, IStandardMappingService standardMappingService)
        {
            _standardService = standardService;
            _standardMappingService = standardMappingService;
        }
        public async Task<ResultObject> Handle(GetStandardsQuery request, CancellationToken cancellationToken)
        {
            var totalItems = await _standardService.CountAsync(request.Filter, cancellationToken);

            if (totalItems == 0) return ResultObject.Ok(GetEmptyPageStandardsResponse(request));

            var procedures = await _standardService.GetAllAsync(request.Filter,
                request.Page,
                request.Count,
                cancellationToken);

            var viewModel = await _standardMappingService.MapToStandardViewModelAsync(procedures, cancellationToken);

            return ResultObject.Ok(BuildFirstPageStandardsResponse(request, totalItems, viewModel));
        }
        private static GetStandardsResponse BuildFirstPageStandardsResponse(GetStandardsQuery query, long totalItems, List<StandardViewModel> items)
        {
            var pageCount = totalItems / query.Count;

            if (items.Count % query.Count > 0)
            {
                pageCount += 1;
            }

            return new GetStandardsResponse
            {
                TotalItems = totalItems,
                TotalPages = pageCount,
                PageNumber = query.Page,
                PageSize = query.Count,
                Items = items
            };
        }
        private static GetStandardsResponse GetEmptyPageStandardsResponse(GetStandardsQuery query)
        {
            return new GetStandardsResponse
            {
                TotalItems = 0,
                TotalPages = 1,
                PageNumber = query.Page,
                PageSize = query.Count,
                Items = new List<StandardViewModel>()
            };
        }
    }
}
