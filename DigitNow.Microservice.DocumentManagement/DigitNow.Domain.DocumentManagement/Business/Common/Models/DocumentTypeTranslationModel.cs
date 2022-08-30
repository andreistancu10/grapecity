using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models
{
    public class DocumentTypeTranslationModel
    {
        public DocumentType DocumentType { get; set; }
        public string Translation { get; set; }
    }
}
