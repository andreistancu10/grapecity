using System;

namespace DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Models
{
    public class CreateWorkflowDecisionRequest
    {
        public int InitiatorType { get; set; }
        public int ActionType { get; set; }
        public string Remarks { get; set; }
        public string DeclineReason { get; set; }
        public int RecipientId { get; set; }
        public string Resolution { get; set; }
        public DateTime? OpionionRequestedUntil { get; set; }
    }
}
