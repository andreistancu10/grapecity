
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetByRegistrationFilter
{
    public class GetDocumentByRegistrationFilterResponse
    {
        public int DocumentId { get; set; }

        public int RegistrationNumber { get; set; }

        public DocumentType DocumentType { get; set; }
    }
}
