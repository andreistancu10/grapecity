using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Dtos
{
    public class WorkflowHistoryLogDto
    {
        public long DocumentId { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public int RecipientType { get; set; }
        public long RecipientId { get; set; }
        public string RecipientName { get; set; }
        public string Remarks { get; set; }
        public string DeclineReason { get; set; }
        public int? Resolution { get; set; }
        public DateTime? OpinionRequestedUntil { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
