using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using System;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class WorkflowHistory : ExtendedEntity
    {
        public int RecipientType { get; set; }
        public long RecipientId { get; set; }
        public string RecipientName { get; set; }
        public string Remarks { get; set; }
        public string DeclineReason { get; set; }
        public int? Resolution { get; set; }
        public DateTime? OpinionRequestedUntil { get; set; }
        public DocumentStatus Status { get; set; }
    }
}
