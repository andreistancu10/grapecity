using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Procedures.Queries.GetFilteredProcedures
{
    public class GetFilteredProceduresHandler : IQueryHandler<GetFilteredProceduresQuery, ResultObject>
    {
        private readonly IProcedureService _procedureService;
        private readonly IProcedureMappingService _procedureMappingService;

        public GetFilteredProceduresHandler(IProcedureService procedureService, IProcedureMappingService procedureMappingService)
        {
            _procedureService = procedureService;
            _procedureMappingService = procedureMappingService;
        }
        public async Task<ResultObject> Handle(GetFilteredProceduresQuery request, CancellationToken cancellationToken)
        {
            var totalItems = await _procedureService.CountAsync(request.Filter, cancellationToken);

            if (totalItems == 0) return ResultObject.Ok(GetEmptyPageProceduresResponse(request));

            var procedures = await _procedureService.GetAllAsync(request.Filter,
                request.Page,
                request.Count,
                cancellationToken);

            var viewModel = await _procedureMappingService.MapToProcedureViewModelAsync(procedures, cancellationToken);

            return ResultObject.Ok(BuildFirstPageProceduresResponse(request, totalItems, viewModel));
        }
        private static GetFilteredProceduresResponse BuildFirstPageProceduresResponse(GetFilteredProceduresQuery query, long totalItems, List<ProcedureViewModel> items)
        {
            var pageCount = totalItems / query.Count;

            if (items.Count % query.Count > 0)
            {
                pageCount += 1;
            }

            return new GetFilteredProceduresResponse
            {
                TotalItems = totalItems,
                TotalPages = pageCount,
                PageNumber = query.Page,
                PageSize = query.Count,
                Items = items
            };
        }
        private static GetFilteredProceduresResponse GetEmptyPageProceduresResponse(GetFilteredProceduresQuery query)
        {
            return new GetFilteredProceduresResponse
            {
                TotalItems = 0,
                TotalPages = 1,
                PageNumber = query.Page,
                PageSize = query.Count,
                Items = new List<ProcedureViewModel>()
            };
        }
    }
}
