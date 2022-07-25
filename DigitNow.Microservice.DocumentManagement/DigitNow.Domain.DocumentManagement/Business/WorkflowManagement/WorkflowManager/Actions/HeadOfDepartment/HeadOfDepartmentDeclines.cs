using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.HeadOfDepartment
{
    public class HeadOfDepartmentDeclines : BaseWorkflowManager, IWorkflowHandler
    {
        public HeadOfDepartmentDeclines(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override int[] allowedTransitionStatuses => new int[] { (int)DocumentStatus.InWorkUnallocated, (int)DocumentStatus.OpinionRequestedUnallocated, (int)DocumentStatus.InWorkDelegatedUnallocated };
        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, VirtualDocument virtualDocument, WorkflowHistory lastWorkFlowRecord, CancellationToken token)
        {
            if (!Validate(command, lastWorkFlowRecord))
                return command;

            switch (document.DocumentType)
            {
                case DocumentType.Incoming:
                    return await CreateWorkflowForIncoming(command, virtualDocument, lastWorkFlowRecord, token);
                case DocumentType.Internal:
                case DocumentType.Outgoing:
                    return await CreateWorkflowForOutgoingAndInternal(command, virtualDocument, lastWorkFlowRecord, token);
                default:
                    return command;
            }
        }

        private async Task<ICreateWorkflowHistoryCommand> CreateWorkflowForOutgoingAndInternal(ICreateWorkflowHistoryCommand command, VirtualDocument virtualDocument, WorkflowHistory lastWorkFlowRecord, CancellationToken token)
        {
            var newWorkflowResponsible = new WorkflowHistory
            {
                Status = DocumentStatus.New,
                DeclineReason = command.DeclineReason,
                Remarks = command.Remarks
            };

            await PassDocumentToFunctionary(virtualDocument, newWorkflowResponsible, command);

            return command;
        }

        private async Task<ICreateWorkflowHistoryCommand> CreateWorkflowForIncoming(ICreateWorkflowHistoryCommand command, VirtualDocument virtualDocument, WorkflowHistory lastWorkFlowRecord, CancellationToken token)
        {
            var newDocumentStatus = lastWorkFlowRecord.Status == DocumentStatus.OpinionRequestedUnallocated
                ? DocumentStatus.InWorkAllocated
                : DocumentStatus.NewDeclinedCompetence;

            await SetResponsibleBasedOnStatus(command, virtualDocument, newDocumentStatus, token);

            return command;
        }

        private async Task SetResponsibleBasedOnStatus(ICreateWorkflowHistoryCommand command, VirtualDocument document, DocumentStatus status, CancellationToken token)
        {
            if (status == DocumentStatus.InWorkAllocated)
            {
                var newWorkflowResponsible = new WorkflowHistory
                {
                    Status = DocumentStatus.InWorkAllocated,
                    DeclineReason = command.DeclineReason,
                    Remarks = command.Remarks
                };

                await PassDocumentToFunctionary(document, newWorkflowResponsible, command);
            }
            else
                await PassDocumentToRegistry(document, command, token);
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistory lastWorkFlowRecord)
        {
            if (lastWorkFlowRecord == null || !allowedTransitionStatuses.Contains((int)lastWorkFlowRecord.Status))
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"Transition not allwed!",
                    TranslationCode = "dms.invalidState.backend.update.validation.invalidState",
                    Parameters = new object[] { command.Resolution }
                });
                return false;
            }

            if (string.IsNullOrWhiteSpace(command.DeclineReason))
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"The reason of decline was not specified.",
                    TranslationCode = "dms.declineReason.backend.update.validation.notSpecified",
                    Parameters = new object[] { command.DeclineReason }
                });
                return false;
            }

            return true;
        }
    }
}
