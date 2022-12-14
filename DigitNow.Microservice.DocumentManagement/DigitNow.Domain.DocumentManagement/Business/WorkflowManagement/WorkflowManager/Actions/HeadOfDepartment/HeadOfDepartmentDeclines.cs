using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.HeadOfDepartment
{
    public class HeadOfDepartmentDeclines : BaseWorkflowManager, IWorkflowHandler
    {
        public HeadOfDepartmentDeclines(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override int[] allowedTransitionStatuses => new int[] 
        { 
            (int)DocumentStatus.InWorkUnallocated, 
            (int)DocumentStatus.OpinionRequestedUnallocated, 
            (int)DocumentStatus.InWorkDelegatedUnallocated 
        };

        #region [ IWorkflowHandler ]
        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, WorkflowHistoryLog lastWorkflowRecord, CancellationToken token)
        {
            if (!Validate(command, lastWorkflowRecord))
                return command;

            switch (document.DocumentType)
            {
                case DocumentType.Incoming:
                    return await CreateWorkflowForIncomingAsync(command, document, lastWorkflowRecord, token);
                case DocumentType.Internal:
                case DocumentType.Outgoing:
                    return await CreateWorkflowForOutgoingAndInternalAsync(command, document, lastWorkflowRecord, token);
                default:
                    return command;
            }
        }

        #endregion

        private async Task<ICreateWorkflowHistoryCommand> CreateWorkflowForOutgoingAndInternalAsync(ICreateWorkflowHistoryCommand command, Document document, WorkflowHistoryLog lastWorkFlowRecord, CancellationToken token)
        {
            var oldWorkflowResponsible = GetOldWorkflowResponsibleAsync(document, x => x.RecipientType == RecipientType.Department.Id);

            document.Status = DocumentStatus.New;
            document.DestinationDepartmentId = oldWorkflowResponsible.RecipientId;

            await PassDocumentToDepartment(document, command, token);
            DeleteActionAfterBeingProcessed(document, UserActionsOnDocument.AsksForOpinion);

            await SendHeadOfDepartmentDeclinesMail(document, lastWorkFlowRecord, token);

            return command;
        }

        private async Task<ICreateWorkflowHistoryCommand> CreateWorkflowForIncomingAsync(ICreateWorkflowHistoryCommand command, Document document, WorkflowHistoryLog lastWorkFlowRecord, CancellationToken token)
        {
            var newDocumentStatus = lastWorkFlowRecord.DocumentStatus == DocumentStatus.OpinionRequestedUnallocated
                ? DocumentStatus.InWorkAllocated
                : DocumentStatus.NewDeclinedCompetence;

            await SetResponsibleBasedOnStatus(command, document, newDocumentStatus, token);
            DeleteActionAfterBeingProcessed(document, UserActionsOnDocument.AsksForOpinion);

            await SendHeadOfDepartmentDeclinesMail(document, lastWorkFlowRecord, token);

            return command;
        }

        private async Task SendHeadOfDepartmentDeclinesMail(Document document, WorkflowHistoryLog historyLog, CancellationToken token)
        {
            if (historyLog.DocumentStatus == DocumentStatus.OpinionRequestedUnallocated)
            {
                var opinionReguestedUnallocatedFlow = document.WorkflowHistories.OrderByDescending(x => x.CreatedAt).FirstOrDefault(x => x.DocumentStatus == DocumentStatus.OpinionRequestedUnallocated);
                if(opinionReguestedUnallocatedFlow != null)
                {
                    await MailSenderService.SendMail_OnCompetenceDeclinedOnOpinionRequested(document, historyLog.DestinationDepartmentId, opinionReguestedUnallocatedFlow, token);
                }
            }
            else
            {
                await MailSenderService.SendMail_OnCompetenceDeclined(document, historyLog.DestinationDepartmentId, token);
            }
        }

        private async Task SetResponsibleBasedOnStatus(ICreateWorkflowHistoryCommand command, Document document, DocumentStatus status, CancellationToken token)
        {
            if (status == DocumentStatus.InWorkAllocated)
            {
                var newWorkflowResponsible = new WorkflowHistoryLog
                {
                    DocumentStatus = status,
                    DeclineReason = command.DeclineReason,
                    Remarks = command.Remarks
                };

                await SendOpinionBackToRequesterAsync(document, newWorkflowResponsible, command, token);
            }
            else
            {
                document.Status = DocumentStatus.NewDeclinedCompetence;
                await PassDocumentToRegistry(document, command, token);
            }
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistoryLog lastWorkFlowRecord)
        {
            if (lastWorkFlowRecord == null || !allowedTransitionStatuses.Contains((int)lastWorkFlowRecord.DocumentStatus))
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
