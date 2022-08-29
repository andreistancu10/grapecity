using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates
{
    public class DynamicFormControlAggregate
    {
        public List<DynamicFormField> DynamicFormFields { get; set; }
        public DynamicFormFieldMapping DynamicFormFieldMapping { get; set; }
    }
}