using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    public class DynamicFormControlAggregate
    {
        public DynamicFormFieldMapping DynamicFormFieldMapping { get; set; }
        public List<DynamicFormField> DynamicFormFields { get; set; }
        public List<DynamicFormFieldValue> DynamicFormFieldsValues { get; set; }
    }
}