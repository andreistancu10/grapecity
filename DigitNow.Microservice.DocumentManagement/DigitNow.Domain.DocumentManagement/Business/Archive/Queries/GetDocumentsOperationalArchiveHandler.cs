using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Archive.Queries
{

    public class GetDocumentsOperationalArchiveHandler : IQueryHandler<GetDocumentsOperationalArchiveQuery, GetDocumentsOperationalArchiveResponse>
    {
        private readonly IDashboardService _dashboardService;
        private readonly IDocumentMappingService _documentMappingService;

        public GetDocumentsOperationalArchiveHandler(
            IDashboardService dashboardService,
            IDocumentMappingService documentMappingService)
        {
            _dashboardService = dashboardService;
            _documentMappingService = documentMappingService;
        }

        public async Task<GetDocumentsOperationalArchiveResponse> Handle(GetDocumentsOperationalArchiveQuery request, CancellationToken cancellationToken)
        {
            var totalItems = await _dashboardService.CountArchivedDocumentsAsync(request.Filter, cancellationToken);

            var documents = await _dashboardService.GetArchivedDocumentsAsync(request.Filter,
                request.Page,
                request.Count,
                cancellationToken);

            var viewModels = await _documentMappingService.MapToDocumentViewModelAsync(request.LanguageId, documents, cancellationToken);

            return BuildFirstPageDocumentResponse(request, totalItems, viewModels);
        }

        private static GetDocumentsOperationalArchiveResponse BuildFirstPageDocumentResponse(GetDocumentsOperationalArchiveQuery query, long totalItems, IList<DocumentViewModel> items)
        {
            var pageCount = totalItems / query.Count;

            if (items.Count % query.Count > 0)
            {
                pageCount += 1;
            }

            return new GetDocumentsOperationalArchiveResponse
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
