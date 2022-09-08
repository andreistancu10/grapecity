using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.GetDynamicFormById
{
    public class GetDynamicFormByIdHandler : IQueryHandler<GetDynamicFormByIdQuery, DynamicFormViewModel>
    {
        private readonly IDynamicFormsService _dynamicFormsService;

        public GetDynamicFormByIdHandler(
            IDynamicFormsService dynamicFormsService)
        {
            _dynamicFormsService = dynamicFormsService;
        }

        public Task<DynamicFormViewModel> Handle(GetDynamicFormByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.FormFillingId.HasValue)
            {
                return _dynamicFormsService.GetDynamicFormViewModelAsync(request.FormId, request.FormFillingId.Value, cancellationToken);
            }
            return _dynamicFormsService.GetDynamicFormViewModelAsync(request.FormId, default, cancellationToken);
        }
    }
}