using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;

namespace DigitNow.Domain.DocumentManagement.Business.DynamicForms.Dtos
{
    public class DynamicFormFieldDto
    {
        public DynamicFieldType DynamicFieldType { get; set; }
        public string Name { get; set; }
        public string Context { get; set; }
    }
}