namespace DigitNow.Domain.DocumentManagement.Public.Forms.Models
{
    public class SaveDynamicFormDataRequest
    {
        public long FormId { get; set; }
        public List<BasicRequestModel> Values { get; set; }
    }
}