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
            try
            {
                await _dynamicFormsService.SaveDataForDynamicFormAsync(request.FormId, request.Values, cancellationToken);
            }
            catch (Exception)
            {
                return ResultObject.CreateError("Error Saving Form Data", "");
            }

            return ResultObject.Ok();
        }
    }
}