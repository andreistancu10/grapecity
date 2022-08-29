namespace DigitNow.Domain.DocumentManagement.Business.DynamicForms.Dtos
{
    public class DynamicFormDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Label { get; set; }
        public string Context { get; set; }

        public List<DynamicFormFieldMappingDto> FieldMappings { get; set; }
    }
}