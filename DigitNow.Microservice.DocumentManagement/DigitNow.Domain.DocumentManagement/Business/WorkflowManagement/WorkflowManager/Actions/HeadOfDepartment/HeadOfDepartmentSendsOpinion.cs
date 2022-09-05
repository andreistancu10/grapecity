using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.HeadOfDepartment
{
    public class HeadOfDepartmentSendsOpinion : BaseWorkflowManager, IWorkflowHandler
    {
        public HeadOfDepartmentSendsOpinion(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override int[] allowedTransitionStatuses => new int[]
        {
            (int)DocumentStatus.OpinionRequestedUnallocated
        };

        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, WorkflowHistoryLog lastWorkflowRecord, CancellationToken token)
        {
            var oldWorkflowResponsible = document.WorkflowHistories
                .OrderByDescending(x => x.CreatedAt)
                .Skip(1)
                .FirstOrDefault();

            if (!Validate(command, lastWorkflowRecord, oldWorkflowResponsible))
                return command;

            oldWorkflowResponsible.DocumentStatus = 
                oldWorkflowResponsible.RecipientType == RecipientType.HeadOfDepartment.Id ? DocumentStatus.InWorkUnallocated :
                oldWorkflowResponsible.RecipientType == RecipientType.Functionary.Id ? DocumentStatus.InWorkAllocated 
            : DocumentStatus.New;

            document.Status = oldWorkflowResponsible.DocumentStatus;

            var newWorkflowResponsible = new WorkflowHistoryLog
            {
                DocumentStatus = oldWorkflowResponsible.DocumentStatus,
                Remarks = command.Remarks
            };

            var makeDocumentVisibleForDepartment = document.Status == DocumentStatus.New;

            await TransferUserResponsibilityAsync(oldWorkflowResponsible, newWorkflowResponsible, command, token, makeDocumentVisibleForDepartment);

            document.WorkflowHistories.Add(newWorkflowResponsible);

            ResetDateAsOpinionWasSent(document);
            DeleteActionAfterBeingProcessed(document, UserActionsOnDocument.AsksForOpinion);

            return command;
        }

        private static void ResetDateAsOpinionWasSent(Document document)
        {
            document.WorkflowHistories.ForEach(x => x.OpinionRequestedUntil = null);
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistoryLog lastWorkFlowRecord, WorkflowHistoryLog oldWorkflowResponsible)
        {
            if (string.IsNullOrWhiteSpace(command.Remarks) || !IsTransitionAllowed(lastWorkFlowRecord, allowedTransitionStatuses) || oldWorkflowResponsible == null)
            {
                TransitionNotAllowed(command);
                return false;
            }

            return true;
        }
    }
}
