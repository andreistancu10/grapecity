using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Public.Forms.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IDynamicFormsServices
    {
        Task<List<DynamicFormFieldMapping>> GetDynamicFormFieldMappingsAsync(long dynamicFormId, CancellationToken cancellationToken);
        Task<DynamicForm> GetDynamicFormAsync(long dynamicFormId, CancellationToken cancellationToken);
        Task SaveDataForDynamicFormAsync(long dynamicFormId, List<BasicRequestModel> values, CancellationToken cancellationToken);
    }

    public class DynamicFormsServices : IDynamicFormsServices
    {
        private readonly DocumentManagementDbContext _dbContext;

        public DynamicFormsServices(DocumentManagementDbContext context)
        {
            _dbContext = context;
        }

        public async Task<List<DynamicFormFieldMapping>> GetDynamicFormFieldMappingsAsync(long dynamicFormId, CancellationToken cancellationToken)
        {
            return await _dbContext.DynamicFormFieldMappings
                .Where(c => c.FormId == dynamicFormId)
                .Include(c => c.DynamicFormField)
                .ToListAsync(cancellationToken);
        }

        public async Task<DynamicForm> GetDynamicFormAsync(long dynamicFormId, CancellationToken cancellationToken)
        {
            return await _dbContext.DynamicForms.FirstOrDefaultAsync(c => c.Id == dynamicFormId, cancellationToken);
        }

        public async Task SaveDataForDynamicFormAsync(long dynamicFormId, List<BasicRequestModel> values, CancellationToken cancellationToken)
        {
            var formFieldMappings = await GetDynamicFormFieldMappingsAsync(dynamicFormId, cancellationToken);
            var fillingLog = new DynamicFormFillingLog { FormId = dynamicFormId };
            var formValues = new List<DynamicFormFieldValue>();

            await _dbContext.DynamicFormFillingLogs.AddAsync(fillingLog, cancellationToken);

            foreach (var requestValue in values)
            {
                var formFieldMapping = formFieldMappings.FirstOrDefault(c => c.Key.ToLower() == requestValue.Key.ToLower());

                if (formFieldMapping != null)
                {
                    formValues.Add(new DynamicFormFieldValue
                    {
                        FormFieldMappingId = formFieldMapping.Id,
                        DynamicFormFillingLog = fillingLog,
                        Value = requestValue.Value
                    });
                }
            }

            await _dbContext.DynamicFormFieldValues.AddRangeAsync(formValues, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}