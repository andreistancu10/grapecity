using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Models
{
    public class DocumentModel
    {
        public long Id { get; set; }
        public DocumentType DocumentType { get; set; }
    }
}
