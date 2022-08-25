using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.ObjectivesDashboard.Queries.GetGeneralObjectives
{
    public class GetGeneralObjectivesHandler : IQueryHandler<GetGeneralObjectivesQuery, GetGeneralObjectivesResponse>
    {
        private readonly IObjectiveDashboardService _objectiveDashboardService;
        private readonly IObjectiveMappingService _objectiveMappingService;
        private readonly IMapper _mapper;
        public GetGeneralObjectivesHandler(IObjectiveDashboardService objectiveDashboardService, IMapper mapper, IObjectiveMappingService objectiveMappingService)
        {
            _objectiveDashboardService = objectiveDashboardService;
            _objectiveMappingService = objectiveMappingService;
            _mapper = mapper;
        }

        public async Task<GetGeneralObjectivesResponse> Handle(GetGeneralObjectivesQuery request, CancellationToken cancellationToken)
        {
            var totalItems = await _objectiveDashboardService.CountGeneralObjectivesAsync(request.Filter, cancellationToken);

            var documents = await _objectiveDashboardService.GetGeneralObjectivesAsync(request.Filter,
                request.Page,
                request.Count,
                cancellationToken);

            var viewModels = await _objectiveMappingService.MapToGeneralObjectiveViewModelAsync(documents, cancellationToken);

            return BuildFirstPageDocumentResponse(request, totalItems, viewModels);
        }

        private static GetGeneralObjectivesResponse BuildFirstPageDocumentResponse(GetGeneralObjectivesQuery query, long totalItems, IList<GeneralObjectiveViewModel> items)
        {
            var pageCount = totalItems / query.Count;

            if (items.Count % query.Count > 0)
            {
                pageCount += 1;
            }

            return new GetGeneralObjectivesResponse
            {
                TotalItems = totalItems,
                TotalPages = pageCount,
                PageNumber = query.Page,
                PageSize = query.Count,
                Items = items
            };
        }
    }
}
