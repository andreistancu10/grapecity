using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Public.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IDynamicFormsServices
    {
        IIncludableQueryable<DynamicFormFieldMapping, DynamicFormField> GetDynamicFormFieldMappingsQueryable(long dynamicFormId);
        Task<DynamicFormViewModel> GetDynamicFormViewModelAsync(long dynamicFormId, CancellationToken cancellationToken);
        IQueryable<DynamicForm> GetDynamicFormsAsync();
        Task SaveDataForDynamicFormAsync(long dynamicFormId, List<KeyValueRequestModel> values, CancellationToken cancellationToken);
    }

    public class DynamicFormsServices : IDynamicFormsServices
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public DynamicFormsServices(DocumentManagementDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public IIncludableQueryable<DynamicFormFieldMapping, DynamicFormField> GetDynamicFormFieldMappingsQueryable(long dynamicFormId)
        {
            return _dbContext.DynamicFormFieldMappings
                .Where(c => c.DynamicFormId == dynamicFormId)
                .Include(c => c.DynamicFormField);
        }

        public async Task<DynamicFormViewModel> GetDynamicFormViewModelAsync(long dynamicFormId, CancellationToken cancellationToken)
        {
            var dynamicForm = await _dbContext.DynamicForms.FirstOrDefaultAsync(c => c.Id == dynamicFormId, cancellationToken);
            var dynamicFormFieldMappings = await GetDynamicFormFieldMappingsQueryable(dynamicFormId).ToListAsync(cancellationToken);
            var dynamicFormFields = dynamicFormFieldMappings.Select(c => c.DynamicFormField).ToList();
            var dynamicFormControlViewModels = new List<DynamicFormControlViewModel>();

            foreach (var mapping in dynamicFormFieldMappings)
            {
                var dynamicFormControlViewModel = _mapper.Map<DynamicFormControlViewModel>(new DynamicFormControlAggregate
                {
                    DynamicFormFields = dynamicFormFields,
                    DynamicFormFieldMapping = mapping
                });

                dynamicFormControlViewModels.Add(dynamicFormControlViewModel);
            }

            var dynamicFormViewModel = _mapper.Map<DynamicFormViewModel>(dynamicForm);
            dynamicFormViewModel.DynamicFormControls = dynamicFormControlViewModels;

            return dynamicFormViewModel;
        }

        public IQueryable<DynamicForm> GetDynamicFormsAsync()
        {
            return _dbContext.DynamicForms.AsNoTracking();
        }

        public async Task SaveDataForDynamicFormAsync(long dynamicFormId, List<KeyValueRequestModel> values, CancellationToken cancellationToken)
        {
            var dynamicFormFieldMappings = await GetDynamicFormFieldMappingsQueryable(dynamicFormId).ToListAsync(cancellationToken);
            var dynamicFormFillingLog = new DynamicFormFillingLog { DynamicFormId = dynamicFormId };

            await _dbContext.DynamicFormFillingLogs.AddAsync(dynamicFormFillingLog, cancellationToken);

            var dynamicFormFieldValues = new List<DynamicFormFieldValue>();

            foreach (var requestValue in values)
            {
                var dynamicFormFieldMapping = dynamicFormFieldMappings.FirstOrDefault(c => c.Key.ToLower() == requestValue.Key.ToLower());

                if (dynamicFormFieldMapping != null)
                {
                    dynamicFormFieldValues.Add(new DynamicFormFieldValue
                    {
                        DynamicFormFieldMappingId = dynamicFormFieldMapping.Id,
                        DynamicFormFillingLog = dynamicFormFillingLog,
                        Value = requestValue.Value
                    });
                }
            }

            await _dbContext.DynamicFormFieldValues.AddRangeAsync(dynamicFormFieldValues, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}