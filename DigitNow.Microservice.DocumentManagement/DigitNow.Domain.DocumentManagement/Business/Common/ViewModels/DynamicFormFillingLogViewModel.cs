namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class DynamicFormFillingLogViewModel
    {
        public long Id { get; set; }
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public BasicViewModel FormCreator { get; set; }
        public BasicViewModel Department { get; set; }
    }
}
