using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;

namespace DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.GetDynamicFormById
{
    public class GetDynamicFormByIdResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Label { get; set; }
        public string Context { get; set; }
        public List<FormControlViewModel> FormControls { get; set; }
    }
}