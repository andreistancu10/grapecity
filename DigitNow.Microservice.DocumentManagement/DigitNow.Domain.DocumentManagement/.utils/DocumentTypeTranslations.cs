using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;

namespace DigitNow.Domain.DocumentManagement.utils
{
    public class DocumentTypeTranslations : List<DocumentTypeTranslation>
    {
        public string GetTranslation(DocumentType documentType)
        {
            var foundDocumentType = this.FirstOrDefault(x => x.DocumentType == documentType);

            if (foundDocumentType != null)
            {
                return foundDocumentType.DocumentTypeLabel;
            }

            return string.Empty;
        }
    }
}