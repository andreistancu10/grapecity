using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.Functionary
{
    public class FunctionarySendsOpinion : BaseWorkflowManager, IWorkflowHandler
    {
        public FunctionarySendsOpinion(IServiceProvider serviceProvider) : base(serviceProvider) { }
        protected override int[] allowedTransitionStatuses => new int[] { (int)DocumentStatus.OpinionRequestedAllocated };

        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, VirtualDocument virtualDocument, WorkflowHistory oldWorkflowResponsible, CancellationToken token)
        {
            if (!Validate(command, oldWorkflowResponsible))
                return command;

            var responsibleFunctionaryRecord = document.IncomingDocument.WorkflowHistory
                .Where(x => x.RecipientType == UserRole.Functionary.Id && x.Status == DocumentStatus.InWorkAllocated)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault();

            var newWorkflowResponsible = new WorkflowHistory
            {
                Status = document.DocumentType == DocumentType.Incoming ? DocumentStatus.InWorkAllocated : DocumentStatus.New,
                Remarks = command.Remarks
            };

            TransferResponsibility(responsibleFunctionaryRecord, newWorkflowResponsible, command);

            virtualDocument.WorkflowHistory.Add(newWorkflowResponsible);

            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistory lastWorkFlowRecord)
        {
            if (string.IsNullOrWhiteSpace(command.Remarks) || !IsTransitionAllowed(lastWorkFlowRecord, allowedTransitionStatuses))
            {
                TransitionNotAllowed(command);
                return false;
            }

            return true;
        }
    }
}
