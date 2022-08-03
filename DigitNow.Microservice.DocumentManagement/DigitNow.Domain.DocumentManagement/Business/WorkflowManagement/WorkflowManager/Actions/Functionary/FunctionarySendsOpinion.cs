﻿using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.Functionary
{
    public class FunctionarySendsOpinion : BaseWorkflowManager, IWorkflowHandler
    {
        public FunctionarySendsOpinion(IServiceProvider serviceProvider) : base(serviceProvider) { }
        protected override int[] allowedTransitionStatuses => new int[] 
        { 
            (int)DocumentStatus.OpinionRequestedAllocated 
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
                    return await CreateWorkflowHistoryForOutgoingOrInternal(command, document, token);
                default:
                    return command;
            }
        }
        #endregion

        private async Task<ICreateWorkflowHistoryCommand> CreateWorkflowHistoryForOutgoingOrInternal(ICreateWorkflowHistoryCommand command, Document document, CancellationToken token)
        {
            var oldWorkflowResponsible = document.WorkflowHistories
                    .Where(x => x.DocumentStatus == DocumentStatus.New)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefault();

            //TODO Create an abstract factory
            var newWorkflowResponsible = new WorkflowHistoryLog
            {
                DocumentStatus = DocumentStatus.New,
                Remarks = command.Remarks,
                RecipientType = RecipientType.Department.Id,
                RecipientId = oldWorkflowResponsible.DestinationDepartmentId,
                RecipientName = $"Departamentul { await GetDocumentNameByIdAsync(oldWorkflowResponsible.DestinationDepartmentId, token)}",
                DestinationDepartmentId = oldWorkflowResponsible.DestinationDepartmentId
            };

            document.Status = DocumentStatus.New;
            document.DestinationDepartmentId = oldWorkflowResponsible.DestinationDepartmentId;
            document.WorkflowHistories.Add(newWorkflowResponsible);

            ResetDateAsOpinionWasSent(document);

            return await Task.FromResult(command);
        }

        private async Task<ICreateWorkflowHistoryCommand> CreateWorkflowHistoryForIncoming(ICreateWorkflowHistoryCommand command, Document document, WorkflowHistoryLog lastWorkflowRecord, CancellationToken token)
        {
            var oldWorkflowResponsible = document.WorkflowHistories
                .Where(x => x.RecipientType == RecipientType.Functionary.Id && x.DocumentStatus == DocumentStatus.InWorkAllocated)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault();

            document.Status = DocumentStatus.InWorkAllocated;

            var newWorkflowResponsible = new WorkflowHistoryLog
            {
                DocumentStatus = DocumentStatus.InWorkAllocated,
                Remarks = command.Remarks
            };

            await TransferUserResponsibilityAsync(oldWorkflowResponsible, newWorkflowResponsible, command, token);

            document.WorkflowHistories.Add(newWorkflowResponsible);

            ResetDateAsOpinionWasSent(document);

            return command;
        }

        private static void ResetDateAsOpinionWasSent(Document document)
        {
            document.WorkflowHistories.ForEach(x => x.OpinionRequestedUntil = null);
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistoryLog lastWorkFlowRecord)
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
