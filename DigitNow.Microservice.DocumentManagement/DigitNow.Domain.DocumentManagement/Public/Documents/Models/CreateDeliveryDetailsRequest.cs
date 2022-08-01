
namespace DigitNow.Domain.DocumentManagement.Public.Documents.Models
{
    public class CreateDeliveryDetailsRequest
    {
        public int DeliveryMode { get; set; }
        public int? DirectShipping { get; set; }
        public int? Post { get; set; }
    }
}
