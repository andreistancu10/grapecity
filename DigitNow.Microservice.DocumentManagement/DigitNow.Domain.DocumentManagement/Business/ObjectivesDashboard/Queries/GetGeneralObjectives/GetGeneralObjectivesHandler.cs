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
        public GetGeneralObjectivesHandler(IObjectiveDashboardService objectiveDashboardService, IObjectiveMappingService objectiveMappingService)
        {
            _objectiveDashboardService = objectiveDashboardService;
            _objectiveMappingService = objectiveMappingService;
        }

        public async Task<GetGeneralObjectivesResponse> Handle(GetGeneralObjectivesQuery request, CancellationToken cancellationToken)
        {
            var totalItems = await _objectiveDashboardService.CountGeneralObjectivesAsync(request.Filter, cancellationToken);

            var objectives = await _objectiveDashboardService.GetGeneralObjectivesAsync(request.Filter,
                request.Page,
                request.Count,
                cancellationToken);

            var viewModels = await _objectiveMappingService.MapToGeneralObjectiveViewModelAsync(objectives, cancellationToken);

            return BuildFirstPageDocumentResponse(request, totalItems, viewModels);
        }

        private static GetGeneralObjectivesResponse BuildFirstPageDocumentResponse(GetGeneralObjectivesQuery query, long totalItems, IList<BasicGeneralObjectiveViewModel> items)
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
