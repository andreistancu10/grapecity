using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Documents;

namespace DigitNow.Domain.DocumentManagement.Domain.Business.Common.Factories
{
    public static class DocumentResolutionFactory
    {
        public static DocumentResolution Create(Document document, DocumentResolutionType resolutionType, string remarks)
            => new DocumentResolution { DocumentId = document.Id, DocumentType = document.DocumentType, ResolutionType = resolutionType, Remarks = remarks };
    }
}
