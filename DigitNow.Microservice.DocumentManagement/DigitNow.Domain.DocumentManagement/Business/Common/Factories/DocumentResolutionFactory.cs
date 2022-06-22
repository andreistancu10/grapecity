using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Documents;

namespace DigitNow.Domain.DocumentManagement.Domain.Business.Common.Factories
{
    public static class DocumentResolutionFactory
    {
        public static DocumentResolution Create(InternalDocument internalDocument, DocumentResolutionType resolutionType, string remarks)
            => new DocumentResolution { DocumentId = internalDocument.Document.Id, DocumentType = internalDocument.Document.DocumentType, ResolutionType = resolutionType, Remarks = remarks };

        public static DocumentResolution Create(IncomingDocument incomingDocument, DocumentResolutionType resolutionType, string remarks)
            => new DocumentResolution { DocumentId = incomingDocument.Document.Id, DocumentType = incomingDocument.Document.DocumentType, ResolutionType = resolutionType, Remarks = remarks };
    }
}
