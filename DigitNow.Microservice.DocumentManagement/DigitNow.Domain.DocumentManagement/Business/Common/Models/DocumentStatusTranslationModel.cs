using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Models
{
    internal class DocumentStatusTranslationModel
    {
        public DocumentStatus Status { get; set; }
        public string Translation { get; set; }
    }
}
