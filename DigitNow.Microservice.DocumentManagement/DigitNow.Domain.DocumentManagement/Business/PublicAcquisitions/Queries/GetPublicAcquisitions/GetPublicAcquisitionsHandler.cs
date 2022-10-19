using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.PublicAcquisitions.Queries.GetPublicAcquisitions
{
    public class GetPublicAcquisitionsHandler : IQueryHandler<GetPublicAcquisitionsQuery, ResultObject>
    {
        private IPublicAcquisitionService _publicAcquisitionsService;
        private IPublicAcquisitionsMappingService _publicAcquisitionsMappingService;

        public GetPublicAcquisitionsHandler(IPublicAcquisitionService publicAcquisitionsService, IPublicAcquisitionsMappingService publicAcquisitionsMappingService)
        {
            _publicAcquisitionsService = publicAcquisitionsService;
            _publicAcquisitionsMappingService = publicAcquisitionsMappingService;
        }
        public async Task<ResultObject> Handle(GetPublicAcquisitionsQuery request, CancellationToken cancellationToken)
        {
            var totalItems = await _publicAcquisitionsService.CountAsync(request.Filter, cancellationToken);
            
            if (totalItems == 0) return ResultObject.Ok(GetEmptyPagePublicAcquisitionsResponse(request));

            var publicAcquisitionProjects = await _publicAcquisitionsService.GetAllAsync(request.Filter,
                request.Page,
                request.Count,
                cancellationToken);

            var viewModel = await _publicAcquisitionsMappingService.MapToPublicAcquisitionViewModelAsync(publicAcquisitionProjects, cancellationToken);

            return ResultObject.Ok(BuildFirstPagePublicAcquisitionsResponse(request, totalItems, viewModel));
        }

        private static GetPublicAcquisitionsResponse BuildFirstPagePublicAcquisitionsResponse(GetPublicAcquisitionsQuery query, long totalItems, List<PublicAcquisitionViewModel> items)
        {
            var pageCount = totalItems / query.Count;

            if (items.Count % query.Count > 0)
            {
                pageCount += 1;
            }

            return new GetPublicAcquisitionsResponse
            {
                TotalItems = totalItems,
                TotalPages = pageCount,
                PageNumber = query.Page,
                PageSize = query.Count,
                Items = items
            };
        }

        private static GetPublicAcquisitionsResponse GetEmptyPagePublicAcquisitionsResponse(GetPublicAcquisitionsQuery query)
        {
            return new GetPublicAcquisitionsResponse
            {
                TotalItems = 0,
                TotalPages = 1,
                PageNumber = query.Page,
                PageSize = query.Count,
                Items = new List<PublicAcquisitionViewModel>()
            };
        }
    }
}
