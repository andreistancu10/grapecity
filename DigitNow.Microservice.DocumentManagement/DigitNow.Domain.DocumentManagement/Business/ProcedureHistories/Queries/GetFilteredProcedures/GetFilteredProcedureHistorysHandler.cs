using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.ProcedureHistories.Queries.GetFilteredProcedures
{
    public class GetFilteredProcedureHistoriesHandler : IQueryHandler<GetFilteredProcedureHistoriesQuery, ResultObject>
    {
        private readonly IProcedureHistoryService _procedureHistoryService;
        private readonly IProcedureHistoryMappingService _procedureHistoryMappingService;

        public GetFilteredProcedureHistoriesHandler(IProcedureHistoryService procedureHistoryService, 
            IProcedureHistoryMappingService procedureHistoryMappingService)
        {
            _procedureHistoryService = procedureHistoryService;
            _procedureHistoryMappingService = procedureHistoryMappingService;
        }

        public async Task<ResultObject> Handle(GetFilteredProcedureHistoriesQuery request, CancellationToken cancellationToken)
        {
            var totalItems = await _procedureHistoryService.CountAsync(request.Filter, cancellationToken);

            if (totalItems == 0)
            {
                return ResultObject.Ok(GetEmptyPageProcedureHistoriesResponse(request));
            }

            var procedureHistories = await _procedureHistoryService.GetAllAsync(request.Filter,
                request.Page,
                request.Count,
                cancellationToken);

            var viewModel = await _procedureHistoryMappingService.MapToProcedureHistoryViewModelAsync(procedureHistories, cancellationToken);

            return ResultObject.Ok(BuildFirstPageProcedureHistoriesResponse(request, totalItems, viewModel));
        }

        private static GetFilteredProcedureHistoriesResponse BuildFirstPageProcedureHistoriesResponse(GetFilteredProcedureHistoriesQuery query, long totalItems, List<ProcedureHistoryViewModel> items)
        {
            var pageCount = totalItems / query.Count;

            if (items.Count % query.Count > 0)
            {
                pageCount += 1;
            }

            return new GetFilteredProcedureHistoriesResponse
            {
                TotalItems = totalItems,
                TotalPages = pageCount,
                PageNumber = query.Page,
                PageSize = query.Count,
                Items = items
            };
        }

        private static GetFilteredProcedureHistoriesResponse GetEmptyPageProcedureHistoriesResponse(GetFilteredProcedureHistoriesQuery query)
        {
            return new GetFilteredProcedureHistoriesResponse
            {
                TotalItems = 0,
                TotalPages = 1,
                PageNumber = query.Page,
                PageSize = query.Count,
                Items = new List<ProcedureHistoryViewModel>()
            };
        }
    }
}
