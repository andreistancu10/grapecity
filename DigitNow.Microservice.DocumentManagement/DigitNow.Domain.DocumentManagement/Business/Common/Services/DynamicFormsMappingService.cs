using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IDynamicFormsMappingService
    {
        DynamicFormViewModel MapToDynamicFormViewModel(
            DynamicForm dynamicForm,
            List<DynamicFormFieldMapping> dynamicFormFieldMappings,
            List<DynamicFormField> dynamicFormFields);

        DynamicFormViewModel MapToDynamicFormViewModel(
            DynamicForm dynamicForm,
            List<DynamicFormFieldMapping> dynamicFormFieldMappings,
            List<DynamicFormField> dynamicFormFields,
            List<DynamicFormFieldValue> dynamicFormFieldValues);
    }

    public class DynamicFormsMappingService : IDynamicFormsMappingService
    {
        #region [ Fields ]

        private readonly IMapper _mapper;

        #endregion

        #region [ Construction ]

        public DynamicFormsMappingService(IMapper mapper)
        {
            _mapper = mapper;
        }

        #endregion

        #region [ IDynamicFormsMappingService ]

        public DynamicFormViewModel MapToDynamicFormViewModel(
            DynamicForm dynamicForm,
            List<DynamicFormFieldMapping> dynamicFormFieldMappings,
            List<DynamicFormField> dynamicFormFields) 
            =>
            MapToDynamicFormViewModel(
                dynamicForm,
                dynamicFormFieldMappings,
                dynamicFormFields,
                default);

        public DynamicFormViewModel MapToDynamicFormViewModel(
            DynamicForm dynamicForm,
            List<DynamicFormFieldMapping> dynamicFormFieldMappings, 
            List<DynamicFormField> dynamicFormFields, 
            List<DynamicFormFieldValue> dynamicFormFieldValues)
        {
            return MapDynamicForm(dynamicForm, dynamicFormFieldMappings, dynamicFormFields, dynamicFormFieldValues);
        }

        #endregion

        #region [ DynamicForms - Utils ]

        private DynamicFormViewModel MapDynamicForm(
            DynamicForm dynamicForm,
            List<DynamicFormFieldMapping> dynamicFormFieldMappings,
            List<DynamicFormField> dynamicFormFields,
            List<DynamicFormFieldValue> dynamicFormFieldValues)
        {
            var dynamicFormViewModel = _mapper.Map<DynamicFormViewModel>(dynamicForm);

            foreach (var mapping in dynamicFormFieldMappings)
            {
                var aggregate = new DynamicFormControlAggregate
                {
                    DynamicFormFields = dynamicFormFields,
                    DynamicFormFieldsValues = dynamicFormFieldValues,
                    DynamicFormFieldMapping = mapping
                };

                dynamicFormViewModel.DynamicFormControls.Add(_mapper.Map<DynamicFormControlViewModel>(aggregate));
            }

            return dynamicFormViewModel;
        }

        #endregion
    }
}
