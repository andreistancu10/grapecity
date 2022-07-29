
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Dtos
{
    public class ConnectedDocumentDto
    {
        public long DocumentId { get; set; }
        public long RegistrationNumber { get; set; }
        public DocumentType DocumentType { get; set; }
    }
}
