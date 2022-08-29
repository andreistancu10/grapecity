using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.GetDynamicFormById
{
    public class GetDynamicFormByIdHandler : IQueryHandler<GetDynamicFormByIdQuery, GetDynamicFormByIdResponse>
    {
        private readonly IMapper _mapper;
        private readonly IDynamicFormsServices _dynamicFormsService;

        public GetDynamicFormByIdHandler(
            IMapper mapper,
            IDynamicFormsServices dynamicFormsService)
        {
            _mapper = mapper;
            _dynamicFormsService = dynamicFormsService;
        }

        public async Task<GetDynamicFormByIdResponse> Handle(GetDynamicFormByIdQuery request, CancellationToken cancellationToken)
        {
            var form = _dynamicFormsService.GetDynamicFormAsync(request.Id, cancellationToken);

            if (form == null)
            {
                return null;
            }

            var formFieldMappings = await _dynamicFormsService.GetDynamicFormFieldMappingsAsync(request.Id, cancellationToken);
            var formFields = formFieldMappings.Select(c => c.DynamicFormField).ToList();

            var formControlViewModels = new List<FormControlViewModel>();

            foreach (var mapping in formFieldMappings)
            {
                var viewModel = _mapper.Map<FormControlViewModel>(new FormControlAggregate
                {
                    FormFields = formFields,
                    DynamicFormFieldMapping = mapping
                });

                formControlViewModels.Add(viewModel);
            }

            var formResponse = _mapper.Map<GetDynamicFormByIdResponse>(form);
            formResponse.FormControls = formControlViewModels;

            return formResponse;
        }
    }
}