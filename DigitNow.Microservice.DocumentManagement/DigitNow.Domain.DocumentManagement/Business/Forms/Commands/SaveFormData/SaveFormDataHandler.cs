using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Forms.Commands
{
    public class SaveFormDataHandler : ICommandHandler<SaveFormDataCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IFormsServices _formsService;

        public SaveFormDataHandler(
            DocumentManagementDbContext dbContext,
            IFormsServices formsService)
        {
            _dbContext = dbContext;
            _formsService = formsService;
        }

        public async Task<ResultObject> Handle(SaveFormDataCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var formFieldMappings = await _formsService.GetFormFieldMappingsByFormIdAsync(request.FormId, cancellationToken);
                var fillingLog = new FormFillingLog { FormId = request.FormId };
                var formValues = new List<FormFieldValue>();

                await _dbContext.FormFillingLogs.AddAsync(fillingLog, cancellationToken);

                foreach (var requestValue in request.Values)
                {
                    var formFieldMapping = formFieldMappings.FirstOrDefault(c => c.Key.ToLower() == requestValue.Key.ToLower());

                    if (formFieldMapping != null)
                    {
                        formValues.Add(new FormFieldValue
                        {
                            FormFieldMappingId = formFieldMapping.Id,
                            FormFillingLog = fillingLog,
                            Value = requestValue.Value
                        });
                    }
                }

                await _dbContext.FormFieldValues.AddRangeAsync(formValues, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                return ResultObject.CreateError("Error Saving Form Data", "");
            }

            return ResultObject.Ok();
        }
    }
}