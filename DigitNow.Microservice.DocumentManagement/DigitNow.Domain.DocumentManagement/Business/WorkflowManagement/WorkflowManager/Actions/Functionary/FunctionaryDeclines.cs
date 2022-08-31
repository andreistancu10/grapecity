using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
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

                await SendOpinionBackToRequesterAsync(document, newWorkflowResponsible, command, token);
            }
            else
            {
                document.Status = DocumentStatus.NewDeclinedCompetence;
                await PassDocumentToRegistry(document, command, token);
            }

            DeleteActionAfterBeingProcessed(document, UserActionsOnDocument.AsksForOpinion);
            await SendFunctonaryDeclinesMail(document, lastWorkFlowRecord, token);

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

            var oldWorkflowResponsible = document.WorkflowHistories
                .Where(x => x.DocumentStatus == DocumentStatus.New)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault();

            await TransferUserResponsibilityAsync(oldWorkflowResponsible, newWorkflowResponsible, command, token);
            document.WorkflowHistories.Add(newWorkflowResponsible);

            var opinionAllocatedFlow = document.WorkflowHistories
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault(x => x.DocumentStatus == DocumentStatus.OpinionRequestedAllocated);

            DeleteActionAfterBeingProcessed(document, UserActionsOnDocument.AsksForOpinion);
            await SendFunctonaryDeclinesMail(document, opinionAllocatedFlow, token);

            return command;
        }

        private async Task SendFunctonaryDeclinesMail(Document document, WorkflowHistoryLog historyLog, CancellationToken token)
        {
            if (historyLog.DocumentStatus == DocumentStatus.OpinionRequestedAllocated)
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
