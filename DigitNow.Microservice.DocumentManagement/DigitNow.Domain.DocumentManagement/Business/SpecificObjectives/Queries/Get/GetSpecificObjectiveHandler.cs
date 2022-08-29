using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Queries.Get
{
    public class GetSpecificObjectiveHandler : IQueryHandler<GetSpecificObjectiveQuery, GetSpecificObjectiveResponse>
    {
        private readonly ISpecificObjectivesDashboardServices _specificObjectiveDashboardService;
        private readonly ISpecificObjectiveMappingService _specificObjectiveMappingService;

        public GetSpecificObjectiveHandler(
            ISpecificObjectivesDashboardServices specificObjectiveService,
            ISpecificObjectiveMappingService specificObjectiveMappingService)
        {
            _specificObjectiveDashboardService = specificObjectiveService;
            _specificObjectiveMappingService = specificObjectiveMappingService;
        }

        public async Task<GetSpecificObjectiveResponse> Handle(GetSpecificObjectiveQuery request, CancellationToken cancellationToken)
        {
            var totalItems = await _specificObjectiveDashboardService.CountSpecificObjectivesAsync(request.Filter, cancellationToken);

            var objectives = await _specificObjectiveDashboardService.GetSpecificObjectivesAsync(request.Filter,
                request.Page,
                request.Count,
                cancellationToken);

            var viewModels = await _specificObjectiveMappingService.MapToSpecificObjectiveViewModelAsync(objectives, cancellationToken);

            return BuildFirstPageSpecificObjectiveResponse(request, totalItems, viewModels);
        }

        private static GetSpecificObjectiveResponse BuildFirstPageSpecificObjectiveResponse(GetSpecificObjectiveQuery query, long totalItems, IList<SpecificObjectiveViewModel> items)
        {
            var pageCount = totalItems / query.Count;

            if (items.Count % query.Count > 0)
            {
                pageCount += 1;
            }

            return new GetSpecificObjectiveResponse
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
