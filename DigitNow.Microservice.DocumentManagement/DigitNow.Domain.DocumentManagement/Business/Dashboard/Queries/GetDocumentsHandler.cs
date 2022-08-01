using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;

public class GetDocumentsHandler : IQueryHandler<GetDocumentsQuery, GetDocumentsResponse>
{
    private readonly IDashboardService _dashboardService;
    private readonly IDocumentMappingService _documentMappingService;

    public GetDocumentsHandler(
        IDashboardService dashboardService,
        IDocumentMappingService documentMappingService)
    {
        _dashboardService = dashboardService;
        _documentMappingService = documentMappingService;
    }

    public async Task<GetDocumentsResponse> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
    {
        var totalItems = await _dashboardService.CountActiveDocumentsAsync(request.Filter, cancellationToken);

        var documents = await _dashboardService.GetActiveDocumentsAsync(request.Filter,
            request.Page, 
            request.Count, 
            cancellationToken);

        var viewModels = await _documentMappingService.MapToDocumentViewModelAsync(documents, cancellationToken);

        return BuildFirstPageDocumentResponse(request, totalItems, viewModels);
    }

    private static GetDocumentsResponse BuildFirstPageDocumentResponse(GetDocumentsQuery query, long totalItems, IList<DocumentViewModel> items)
    {
        var pageCount = totalItems / query.Count;

        if (items.Count % query.Count > 0)
        {
            pageCount += 1;
        }

        return new GetDocumentsResponse
        {
            TotalItems = totalItems,
            TotalPages = pageCount,
            PageNumber = query.Page,
            PageSize = query.Count,
            Items = items
        };
    }
}