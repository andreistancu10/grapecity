using DigitNow.Domain.DocumentManagement.Contracts.DynamicForms;

namespace DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.Common
{
    public class DynamicFormFieldResponse
    {
        public DynamicFieldType DynamicFieldType { get; set; }
        public string Name { get; set; }
        public string Context { get; set; }
    }
}