using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using HTSS.Platform.Core.CQRS;
using System;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers._Interfaces
{
    public interface ICreateWorkflowHistoryCommand
    {
        public int RegistrationNumber { get; set; }
        public int InitiatorType { get; set; }
        public int ActionType { get; set; }
        public UserRole RecipientType { get; set; }
        public int RecipientId { get; set; }
        public string Remarks { get; set; }
        public string DeclineReason { get; set; }
        public int Resolution { get; set; }
        public int Decision { get; set; }
        public DateTime? OpinionRequestedUntil { get; set; }
        public DocumentStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
        public ResultObject Result { get; set; }
        public bool RecipientHasChanged { get; set; }
    }
}