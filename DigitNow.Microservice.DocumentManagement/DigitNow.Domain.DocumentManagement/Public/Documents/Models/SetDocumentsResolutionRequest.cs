using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Public.Documents.Models
{
    public class SetDocumentsResolutionRequest
    {
        public DocumentBatchDto Batch { get; set; }

        public DocumentResolutionType Resolution { get; set; }

        public string Remarks { get; set; }
    }

    public class DocumentBatchDto
    {
        public List<DocumentDto> Documents { get; set; }
    }

    public class DocumentDto
    {
        public long Id { get; set; }
        public DocumentType DocumentType { get; set; }
    }
}