namespace DigitNow.Domain.DocumentManagement.Business.Forms.Dtos
{
    public class FormDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Label { get; set; }
        public string Context { get; set; }

        public List<FormFieldMappingDto> FieldMappings { get; set; }
    }
}