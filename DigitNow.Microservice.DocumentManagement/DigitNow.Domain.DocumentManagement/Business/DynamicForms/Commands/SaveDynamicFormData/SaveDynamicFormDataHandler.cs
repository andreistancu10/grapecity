using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.DynamicForms.Commands
{
    public class SaveDynamicFormDataHandler : ICommandHandler<SaveDynamicFormDataCommand, ResultObject>
    {
        private readonly IDynamicFormsServices _dynamicFormsService;

        public SaveDynamicFormDataHandler(
            IDynamicFormsServices dynamicFormsService)
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