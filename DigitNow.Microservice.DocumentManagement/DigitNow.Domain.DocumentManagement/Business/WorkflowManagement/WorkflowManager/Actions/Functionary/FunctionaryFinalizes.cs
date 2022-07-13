using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.Functionary
{
    public class FunctionaryFinalizes : BaseWorkflowManager, IWorkflowHandler
    {
        public FunctionaryFinalizes(IServiceProvider serviceProvider) : base(serviceProvider) { }
        protected override int[] allowedTransitionStatuses => new int[] { (int)DocumentStatus.InWorkAllocated, (int)DocumentStatus.InWorkDelegated };
        
        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var virtualDocument = await GetVirtualDocumentWorkflowHistoryByIdAsync(command.DocumentId, token);
            var oldWorkflowResponsible = GetLastWorkflowRecord(virtualDocument);

            if (!Validate(command, oldWorkflowResponsible))
                return command;

            var newWorkflowResponsible = new WorkflowHistory();
            newWorkflowResponsible.Status = DocumentStatus.Finalized;
            newWorkflowResponsible.Remarks = command.Remarks;

            TransferResponsibility(oldWorkflowResponsible, newWorkflowResponsible, command);

            virtualDocument.WorkflowHistory.Add(newWorkflowResponsible);

            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistory lastWorkFlowRecord)
        {
            if (!IsTransitionAllowed(lastWorkFlowRecord, allowedTransitionStatuses))
            {
                TransitionNotAllowed(command);
                return false;
            }

            return true;
        }
    }
}
