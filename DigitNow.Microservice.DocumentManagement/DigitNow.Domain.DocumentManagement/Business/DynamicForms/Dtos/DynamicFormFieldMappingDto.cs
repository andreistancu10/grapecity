namespace DigitNow.Domain.DocumentManagement.Business.DynamicForms.Dtos
{
    public class DynamicFormFieldMappingDto
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Label { get; set; }
        public string Context { get; set; }
        public int Order { get; set; }
        public bool Required { get; set; }
        public string InitialValue { get; set; }
        public string CurrentValue { get; set; }

        public DynamicFormFieldDto DynamicField { get; set; }
    }
}