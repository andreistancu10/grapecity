using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
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
                    return await CreateWorkflowHistoryForIncoming(command, document, token);
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

            document.Status = DocumentStatus.New;
            document.DestinationDepartmentId = oldWorkflowResponsible.DestinationDepartmentId;

            await PassDocumentToDepartment(document, command, token);

            ResetDateAsOpinionWasSent(document);
            await MailSenderService.SendMail_OpinionFunctionaryReply(document, token);
            return command;
        }

        private async Task<ICreateWorkflowHistoryCommand> CreateWorkflowHistoryForIncoming(ICreateWorkflowHistoryCommand command, Document document, CancellationToken token)
        {
            var oldWorkflowResponsible = document.WorkflowHistories
                .Where(x => (x.DocumentStatus == DocumentStatus.InWorkAllocated) 
                            || x.DocumentStatus == DocumentStatus.InWorkUnallocated 
                            || x.DocumentStatus == DocumentStatus.InWorkDelegatedUnallocated)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault();

            oldWorkflowResponsible.DocumentStatus = oldWorkflowResponsible.RecipientType == RecipientType.HeadOfDepartment.Id
                  ? DocumentStatus.InWorkUnallocated
                  : DocumentStatus.InWorkAllocated;

            document.Status = oldWorkflowResponsible.DocumentStatus;

            var newWorkflowResponsible = new WorkflowHistoryLog
            {
                DocumentStatus = oldWorkflowResponsible.DocumentStatus,
                Remarks = command.Remarks
            };

            await TransferUserResponsibilityAsync(oldWorkflowResponsible, newWorkflowResponsible, command, token);

            document.WorkflowHistories.Add(newWorkflowResponsible);

            ResetDateAsOpinionWasSent(document);
            await MailSenderService.SendMail_OpinionFunctionaryReply(document, token);

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
