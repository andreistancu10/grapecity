using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using System;

namespace DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Models
{
    public class CreateWorkflowDecisionRequest
    {
        public UserRole InitiatorType { get; set; }
        public int ActionType { get; set; }
        public string? Remarks { get; set; }
        public string? DeclineReason { get; set; }
        public int? RecipientId { get; set; }
        public int? Resolution { get; set; }
        public int? Decision { get; set; }
        public DateTime? OpionionRequestedUntil { get; set; }
    }
}
