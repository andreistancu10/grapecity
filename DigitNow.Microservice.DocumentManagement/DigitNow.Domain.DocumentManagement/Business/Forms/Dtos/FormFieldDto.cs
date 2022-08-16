using DigitNow.Domain.DocumentManagement.utils;

namespace DigitNow.Domain.DocumentManagement.Business.Forms.Dtos
{
    public class FormFieldDto
    {
        public FieldType FieldType { get; set; }
        public string Name { get; set; }
        public string Context { get; set; }
    }
}