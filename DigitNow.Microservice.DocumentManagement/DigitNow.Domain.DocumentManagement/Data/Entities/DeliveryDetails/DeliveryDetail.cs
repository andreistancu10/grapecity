
namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class DeliveryDetail : ExtendedEntity
    {
        public int DeliveryMode { get; set; }
        public int DirectShipping { get; set; }
        public int Post { get; set; }
    }
}
