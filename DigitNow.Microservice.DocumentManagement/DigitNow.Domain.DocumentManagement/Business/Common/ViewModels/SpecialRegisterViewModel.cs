namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class SpecialRegisterViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Observations { get; set; }
        public BasicViewModel DocumentCategory { get; set; }
    }
}