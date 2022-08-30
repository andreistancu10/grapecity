using DigitNow.Domain.DocumentManagement.Public.Common.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Forms.Models
{
    public class SaveDynamicFormDataRequest
    {
        public long DynamicFormId { get; set; }
        public List<KeyValueRequestModel> DynamicFormFillingValues { get; set; }
    }
}