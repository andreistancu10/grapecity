using DigitNow.Domain.DocumentManagement.utils;

namespace DigitNow.Domain.DocumentManagement.Business.Forms.Queries.Common
{
    public class FormFieldResponse
    {
        public DynamicFieldType DynamicFieldType { get; set; }
        public string Name { get; set; }
        public string Context { get; set; }
    }
}