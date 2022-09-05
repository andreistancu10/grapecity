using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.GetDynamicForms
{
    public class GetDynamicFormFillingLogsHandler : IQueryHandler<GetDynamicFormFillingLogsQuery, GetDynamicFormFillingLogsResponse>
    {
        private readonly IDynamicFormsService _dynamicFormsService;
        private readonly IDynamicFormsMappingService _dynamicFormMappingService;

        public GetDynamicFormFillingLogsHandler(IDynamicFormsService dynamicFormsService, IDynamicFormsMappingService dynamicFormMappingService)
        {
            _dynamicFormsService = dynamicFormsService;
            _dynamicFormMappingService = dynamicFormMappingService;
        }
        public async Task<GetDynamicFormFillingLogsResponse> Handle(GetDynamicFormFillingLogsQuery request, CancellationToken cancellationToken)
        {
            var totalItems = await _dynamicFormsService.CountDynamicFilledFormsAsync(request.Filter, cancellationToken);

            //ToDo: throw empty obj if totalitems = 0

            var dynamicForms = await _dynamicFormsService.GetDynamicFilledFormsAsync(request.Filter,
                request.Page,
                request.Count,
                cancellationToken);

            var viewModels = await _dynamicFormMappingService.MapToDynamicFormViewModelAsync(dynamicForms, cancellationToken);

            return BuildFirstPageDocumentResponse(request, totalItems, viewModels);
        }

        private static GetDynamicFormFillingLogsResponse BuildFirstPageDocumentResponse(GetDynamicFormFillingLogsQuery query, long totalItems, IList<DynamicFormFillingLogViewModel> items)
        {
            var pageCount = totalItems / query.Count;

            if (items.Count % query.Count > 0)
            {
                pageCount += 1;
            }

            return new GetDynamicFormFillingLogsResponse
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
