using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.Actions._Interfaces;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.Actions.Functionary
{
    public class FunctionaryFinalizes : BaseWorkflowManager, IWorkflowHandler
    {
        private int[] allowedTransitionStatuses = { (int)DocumentStatus.InWorkAllocated, (int)DocumentStatus.InWorkDelegated };
        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var document = await GetDocumentById(command.DocumentId, token);
            var lastWorkFlowRecord = GetLastWorkflowRecord(document);

            if (!Validate(command, lastWorkFlowRecord))
                return command;

            ResetWorkflowRecord(lastWorkFlowRecord);

            lastWorkFlowRecord.Status = DocumentStatus.Finalized;
            lastWorkFlowRecord.Remarks = command.Remarks;

            document.IncomingDocument.WorkflowHistory.Add(lastWorkFlowRecord);
            await SaveDocument(token);

            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistory lastWorkFlowRecord)
        {
            return IsTransitionAllowed(command, lastWorkFlowRecord, allowedTransitionStatuses);
        }
    }
}
