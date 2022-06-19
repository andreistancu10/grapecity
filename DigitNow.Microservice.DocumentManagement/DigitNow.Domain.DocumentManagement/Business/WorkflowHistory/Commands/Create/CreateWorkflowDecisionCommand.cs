using DigitNow.Domain.DocumentManagement.Contracts.Documents;
using DigitNow.Domain.DocumentManagement.Contracts.WorkflowHistory;
using HTSS.Platform.Core.CQRS;
using System;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.Commands.Create
{
    public class CreateWorkflowDecisionCommand : ICreateWorkflowHistoryCommand, ICommand<ResultObject>
    {
        public int RegistrationNumber { get; set; }
        public int RecipientType { get; set; }
        public int RecipientId { get; set; }
        public string RecipientName { get; set; }
        public string Remarks { get; set; }
        public string DeclineReason { get; set; }
        public string Resolution { get; set; }
        public DateTime? OpionionRequestedUntil { get; set; }
        public int Status { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
