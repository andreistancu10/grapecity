using System;

namespace DigitNow.Domain.DocumentManagement.Data.WorkflowHistories
{
    public class WorkflowHistory
    {
        public int Id { get; set; }
        public int RecipientType { get; set; }
        public string RecipientId { get; set; }
        public int? ActionType { get; set; }
        public string Remarks { get; set; }
        public string DeclineReason { get; set; }
        public string Resolution { get; set; }
        public DateTime? OpionionRequestedUntil { get; set; }
        public int Status { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
