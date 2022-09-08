using DigitNow.Domain.DocumentManagement.Contracts.DynamicForms;
using DigitNow.Domain.DocumentManagement.utils;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models
{
    public class DynamicFormFieldModel
    {
        public long Id { get; set; }
        public DynamicFieldType DynamicFieldType { get; set; }
        public string Name { get; set; }
        public string Context { get; set; }
    }
}