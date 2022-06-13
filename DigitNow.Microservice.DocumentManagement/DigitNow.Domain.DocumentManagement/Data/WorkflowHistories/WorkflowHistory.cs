using System;

namespace DigitNow.Domain.DocumentManagement.Data.WorkflowHistories
{
    public class WorkflowHistory
    {
        public int Id { get; set; }
        public int InitiatorType { get; set; }
        public string InitiatorName { get; set; }
        public int ActionType { get; set; }
        public string Remarks { get; set; }
        public string DeclineReason { get; set; }
        public string DestinationType { get; set; }
        public string DestinationName { get; set; }
        public string Resolution { get; set; }
        public DateTime? OpionionRequestedUntil { get; set; }
        public int Status { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
