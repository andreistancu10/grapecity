using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.DynamicForms.Commands
{
    public class SaveDynamicFormDataHandler : ICommandHandler<SaveDynamicFormDataCommand, ResultObject>
    {
        private readonly IDynamicFormsService _dynamicFormsService;

        public SaveDynamicFormDataHandler(
            IDynamicFormsService dynamicFormsService)
        {
            _dynamicFormsService = dynamicFormsService;
        }

        public async Task<ResultObject> Handle(SaveDynamicFormDataCommand request, CancellationToken cancellationToken)
        {
            await _dynamicFormsService.SaveDataForDynamicFormAsync(request.DynamicFormId, request.DynamicFormFillingValues, cancellationToken);

            return ResultObject.Ok();
        }
    }
}