using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.GetDynamicForms
{
    public class GetHistoricalArchiveDocumentsHandler : IQueryHandler<GetHistoricalArchiveDocumentsQuery, GetHistoricalArchiveDocumentsResponse>
    {
        private readonly IDynamicFormsService _dynamicFormsService;
        private readonly IDocumentMappingService _documentMappingService;

        public GetHistoricalArchiveDocumentsHandler(IDynamicFormsService dynamicFormsService, IDocumentMappingService documentMappingService)
        {
            _dynamicFormsService = dynamicFormsService;
            _documentMappingService = documentMappingService;
        }

        public async Task<GetHistoricalArchiveDocumentsResponse> Handle(GetHistoricalArchiveDocumentsQuery request, CancellationToken cancellationToken)
        {
            var totalItems = await _dynamicFormsService.CountDynamicFilledFormsAsync(request.Filter, cancellationToken);
            if (totalItems == 0) return GetEmptyPageDocumentResponse(request);

            var dynamicForms = await _dynamicFormsService.GetDynamicFilledFormsAsync(request.Filter,
                request.Page,
                request.Count,
                cancellationToken);

            var viewModels = await _documentMappingService.MapToHistoricalArchiveDocumentsViewModelAsync(dynamicForms, cancellationToken);

            return BuildFirstPageDocumentResponse(request, totalItems, viewModels);
        }

        private static GetHistoricalArchiveDocumentsResponse BuildFirstPageDocumentResponse(GetHistoricalArchiveDocumentsQuery query, long totalItems, IList<HistoricalArchiveDocumentsViewModel> items)
        {
            var pageCount = totalItems / query.Count;

            if (items.Count % query.Count > 0)
            {
                pageCount += 1;
            }

            return new GetHistoricalArchiveDocumentsResponse
            {
                TotalItems = totalItems,
                TotalPages = pageCount,
                PageNumber = query.Page,
                PageSize = query.Count,
                Items = items
            };
        }

        private static GetHistoricalArchiveDocumentsResponse GetEmptyPageDocumentResponse(GetHistoricalArchiveDocumentsQuery query)
        {
            return new GetHistoricalArchiveDocumentsResponse
            {
                TotalItems = 0,
                TotalPages = 1,
                PageNumber = query.Page,
                PageSize = query.Count,
                Items = new List<HistoricalArchiveDocumentsViewModel>()
            };
        }
    }
}
