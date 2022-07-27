using DigitNow.Domain.DocumentManagement.Data.Entities.DeliveryDetails;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Documents.Abstractions
{
    public interface IShippable
    {
        public DeliveryDetail DeliveryDetails { get; set; }
    }
}
