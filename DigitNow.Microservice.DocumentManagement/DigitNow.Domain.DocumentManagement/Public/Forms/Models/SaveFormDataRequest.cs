namespace DigitNow.Domain.DocumentManagement.Public.Forms.Models
{
    public class SaveFormDataRequest
    {
        public long FormId { get; set; }
        public List<BasicRequestModel> Values { get; set; }
    }
}