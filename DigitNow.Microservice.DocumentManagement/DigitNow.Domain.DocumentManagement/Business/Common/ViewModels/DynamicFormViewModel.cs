namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class DynamicFormViewModel
    {
        public DynamicFormDetailsViewModel DynamicFormDetails { get; set; }
        public List<DynamicFormControlViewModel> DynamicFormControls { get; set; }
    }
}