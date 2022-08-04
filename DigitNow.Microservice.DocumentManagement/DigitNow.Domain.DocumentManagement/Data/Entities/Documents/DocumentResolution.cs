using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class DocumentResolution : ExtendedEntity
    {
        public long DocumentId { get; set; }

        public DocumentType DocumentType { get; set; }

        public DocumentResolutionType ResolutionType { get; set; }

        public string Remarks { get; set; }
    }
}
