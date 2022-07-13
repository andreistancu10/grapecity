using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;
using HTSS.Platform.Core.CQRS;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;

public class GetDocumentsHandler : IQueryHandler<GetDocumentsQuery, GetDocumentsResponse>
{
    private readonly IMapper _mapper;
    private readonly IDashboardService _dashboardService;
    private readonly IDocumentMappingService _documentMappingService;

    public GetDocumentsHandler(IMapper mapper,
        IDashboardService dashboardService,
        IDocumentMappingService documentMappingService)
    {
        _mapper = mapper;
        _dashboardService = dashboardService;
        _documentMappingService = documentMappingService;
    }

    public Task<GetDocumentsResponse> Handle(GetDocumentsQuery query, CancellationToken cancellationToken)
    {
        if (query.PreprocessFilter.IsEmpty() && query.PostprocessFilter.IsEmpty())
        {
            return GetSimpleResponseAsync(query, cancellationToken);
        }
        return GetComplexResponseAsync(query, cancellationToken);
    }

    private async Task<GetDocumentsResponse> GetSimpleResponseAsync(GetDocumentsQuery query, CancellationToken cancellationToken)
    {
        var totalItems = await _dashboardService.CountAllDocumentsAsync(DocumentPreprocessFilter.Empty, DocumentPostprocessFilter.Empty, cancellationToken);

        var documents = await _dashboardService.GetAllDocumentsAsync(DocumentPreprocessFilter.Empty, DocumentPostprocessFilter.Empty,
                query.Page,
                query.Count,
            cancellationToken);

        var viewModels = await _documentMappingService.MapToDocumentViewModelAsync(documents, cancellationToken);

        return BuildDocumentResponse(query, totalItems, viewModels);
    }

    private async Task<GetDocumentsResponse> GetComplexResponseAsync(GetDocumentsQuery query, CancellationToken cancellationToken)
    {
        var totalItems = await _dashboardService.CountAllDocumentsAsync(query.PreprocessFilter, query.PostprocessFilter, cancellationToken);

        var documents = await _dashboardService.GetAllDocumentsAsync(query.PreprocessFilter, query.PostprocessFilter,
            query.Page,
            query.Count,
            cancellationToken);

        var viewModels = await _documentMappingService.MapToDocumentViewModelAsync(documents, cancellationToken);

        return BuildDocumentResponse(query, totalItems, viewModels);
    }

    private GetDocumentsResponse BuildDocumentResponse(GetDocumentsQuery query, long totalItems, IList<DocumentViewModel> items)
    {
        var pageCount = totalItems / query.Count;

        if (items.Count % query.Count > 0)
        {
            pageCount += 1;
        }

        return new GetDocumentsResponse
        {
            TotalItems = totalItems,
            PageNumber = query.Page,
            PageSize = query.Count,
            TotalPages = pageCount,
            Documents = items
        };
    }
}