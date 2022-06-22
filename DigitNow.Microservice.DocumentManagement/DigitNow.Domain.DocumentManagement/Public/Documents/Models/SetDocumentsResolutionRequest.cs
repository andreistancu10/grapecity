using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Public.Documents.Models
{
    public class SetDocumentsResolutionRequest
    {
        public DocumentBatch Batch { get; set; }

        public DocumentResolutionType Resolution { get; set; }

        public string Remarks { get; set; } = null;
    }
}