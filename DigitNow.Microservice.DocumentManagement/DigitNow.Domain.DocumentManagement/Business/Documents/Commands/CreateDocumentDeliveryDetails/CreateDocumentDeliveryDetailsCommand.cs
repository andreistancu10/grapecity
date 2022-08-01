
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Commands.CreateDocumentDeliveryDetails
{
    public class CreateDocumentDeliveryDetailsCommand : ICommand<ResultObject>
    {
        public long DocumentId { get; set; }
        public int DeliveryMode { get; set; }
        public int DirectShipping { get; set; }
        public int Post { get; set; }
    }
}
