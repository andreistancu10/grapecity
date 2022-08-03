using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;

namespace DigitNow.Domain.DocumentManagement.utils
{
    public class DocumentTypeTranslation
    {
        public DocumentType DocumentType { get; set; }
        public string DocumentTypeLabel { get; set; }

        public DocumentTypeTranslation(DocumentType documentType, string documentTypeLabel)
        {
            DocumentType = documentType;
            DocumentTypeLabel = documentTypeLabel;
        }
    }
}