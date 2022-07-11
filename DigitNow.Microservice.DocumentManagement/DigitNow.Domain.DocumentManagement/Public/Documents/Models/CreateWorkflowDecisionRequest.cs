using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using System;

namespace DigitNow.Domain.DocumentManagement.Public.Documents.Models
{
    public class CreateWorkflowDecisionRequest
    {
        public DocumentType DocumentType { get; set; }
        public int InitiatorType { get; set; }
        public int ActionType { get; set; }
        public string? Remarks { get; set; }
        public string? DeclineReason { get; set; }
        public long? RecipientId { get; set; }
        public int? Resolution { get; set; }
        public int? Decision { get; set; }
        public DateTime? OpionionRequestedUntil { get; set; }
    }
}
