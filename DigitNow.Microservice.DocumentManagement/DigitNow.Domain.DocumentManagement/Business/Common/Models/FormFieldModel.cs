using DigitNow.Domain.DocumentManagement.utils;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models
{
    public class FormFieldModel
    {
        public long Id { get; set; }
        public FieldType FieldType { get; set; }
        public string Name { get; set; }
        public string Context { get; set; }
    }
}