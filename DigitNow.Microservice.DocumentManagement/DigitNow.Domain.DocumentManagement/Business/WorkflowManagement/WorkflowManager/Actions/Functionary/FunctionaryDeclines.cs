﻿using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.Functionary
{
    public class FunctionaryDeclines : BaseWorkflowManager, IWorkflowHandler
    {
        public FunctionaryDeclines(IServiceProvider serviceProvider) : base(serviceProvider) { }
        protected override int[] allowedTransitionStatuses => new int[] 
        { 
            (int)DocumentStatus.InWorkAllocated, 
            (int)DocumentStatus.InWorkDelegated, 
            (int)DocumentStatus.OpinionRequestedAllocated, 
            (int)DocumentStatus.InWorkDeclined 
        };

        #region [ IWorkflowHandler ]
        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, WorkflowHistoryLog lastWorkflowRecord, CancellationToken token)
        {
            if (!Validate(command, lastWorkflowRecord))
                return command;

            switch (document.DocumentType)
            {
                case DocumentType.Incoming:
                    return await CreateWorkflowHistoryForIncoming(command, document, lastWorkflowRecord, token);
                case DocumentType.Internal:
                case DocumentType.Outgoing:
                    return await CreateWorkflowHistoryForOutgoingAndInternal(command, document, token);
                default:
                    return command;
            }
        }
        #endregion

        private async Task<ICreateWorkflowHistoryCommand> CreateWorkflowHistoryForIncoming(ICreateWorkflowHistoryCommand command, Document document, WorkflowHistoryLog lastWorkFlowRecord, CancellationToken token)
        {
            var newDocumentStatus = lastWorkFlowRecord.DocumentStatus == DocumentStatus.OpinionRequestedAllocated
                ? DocumentStatus.InWorkAllocated
                : DocumentStatus.NewDeclinedCompetence;

            if (newDocumentStatus == DocumentStatus.InWorkAllocated)
            {
                var newWorkflowResponsible = new WorkflowHistoryLog
                {
                    DocumentStatus = DocumentStatus.InWorkAllocated,
                    DeclineReason = command.DeclineReason,
                    Remarks = command.Remarks
                };

                await PassDocumentToFunctionaryAsync(document, newWorkflowResponsible, command, token);
            }
            else
            {
                document.Status = DocumentStatus.NewDeclinedCompetence;
                await PassDocumentToRegistry(document, command, token);
            }

            return command;
        }

        private async Task<ICreateWorkflowHistoryCommand> CreateWorkflowHistoryForOutgoingAndInternal(ICreateWorkflowHistoryCommand command, Document document, CancellationToken token)
        {
            var newWorkflowResponsible = new WorkflowHistoryLog
            {
                DocumentStatus = DocumentStatus.New,
                DeclineReason = command.DeclineReason,
                Remarks = command.Remarks
            };

            await PassDocumentToFunctionaryAsync(document, newWorkflowResponsible, command, token);

            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistoryLog lastWorkFlowRecord)
        {
            if (string.IsNullOrWhiteSpace(command.DeclineReason) || !IsTransitionAllowed(lastWorkFlowRecord, allowedTransitionStatuses))
            {
                TransitionNotAllowed(command);
                return false;
            }

            return true;
        }
    }
}
