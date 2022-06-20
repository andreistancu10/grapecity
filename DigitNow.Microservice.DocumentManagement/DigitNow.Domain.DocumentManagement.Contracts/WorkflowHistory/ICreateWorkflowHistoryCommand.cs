﻿using System;

namespace DigitNow.Domain.DocumentManagement.Contracts.WorkflowHistory
{
    public interface ICreateWorkflowHistoryCommand
    {
        public int RegistrationNumber { get; set; }
        public int InitiatorType { get; set; }
        public int ActionType { get; set; }
        public int RecipientType { get; set; }
        public int RecipientId { get; set; }
        public string RecipientName { get; set; }
        public string Remarks { get; set; }
        public string DeclineReason { get; set; }
        public string Resolution { get; set; }
        public DateTime? OpionionRequestedUntil { get; set; }
        public int Status { get; set; }
        public DateTime CreationDate { get; set; }
        public object Response { get; set; }
        public bool RecipientHasChanged { get; set; }
    }
}
