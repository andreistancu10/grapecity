using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;
using HTSS.Platform.Core.CQRS;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        var totalItems = await _dashboardService.CountAllDocumentsAsync(request.PreprocessFilter, request.PostprocessFilter, cancellationToken);

        var documents = await _dashboardService.GetAllDocumentsAsync(request.PreprocessFilter, request.PostprocessFilter,
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