using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.HeadOfDepartment
{
    public class HeadOfDepartmentMakesDecision : BaseWorkflowManager, IWorkflowHandler
    {
        public HeadOfDepartmentMakesDecision(IServiceProvider serviceProvider) : base(serviceProvider) { }
        private enum Decision { Approved = 1, Declined = 2 };

        protected override int[] allowedTransitionStatuses => new int[] 
        { 
            (int)DocumentStatus.InWorkApprovalRequested, 
            (int)DocumentStatus.OpinionRequestedUnallocated 
        };

        #region [ IWorkflowHandler ]
        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, WorkflowHistoryLog lastWorkflowRecord, CancellationToken token)
        {
            if (!Validate(command, lastWorkflowRecord))
                return command;

            switch ((Decision)command.Decision)
            {
                case Decision.Declined:
                    await ApplicationDeclinedAsync(command, document, token);
                    break;
                case Decision.Approved:
                    await ApplicationApproved(command, document, token);
                    break;
                default:
                    break;
            }

            return command;
        }

        #endregion

        private async Task<ICreateWorkflowHistoryCommand> ApplicationApproved(ICreateWorkflowHistoryCommand command, Document document, CancellationToken token)
        {
            var userResponse = await IdentityService.GetMayorAsync(token);

            if (!UserExists(userResponse, command))
                return command;

            document.WorkflowHistories
                .Add(WorkflowHistoryLogFactory
                .Create(document, RecipientType.Mayor, userResponse, DocumentStatus.InWorkMayorReview, command.DeclineReason, command.Remarks, command.OpinionRequestedUntil, command.Resolution));

            await UpdateDocumentBasedOnWorkflowDecisionAsync(makeDocumentVisibleForDepartment: false, command.DocumentId, userResponse.Id, DocumentStatus.InWorkMayorReview, token);
            
            await MailSenderService.SendMail_OnDepartmentSupervisorApprovedDecision(document, token);

            return command;
        }

        private async Task ApplicationDeclinedAsync(ICreateWorkflowHistoryCommand command, Document document, CancellationToken token)
        {
            var oldWorkflowResponsible = GetOldWorkflowResponsibleAsync(document,document.DocumentType == DocumentType.Incoming 
                ? x => x.RecipientType == RecipientType.Functionary.Id 
                : x => x.RecipientType == RecipientType.Department.Id);

            var newWorkflowResponsible = new WorkflowHistoryLog
            {
                DocumentStatus = DocumentStatus.InWorkDeclined,
                DeclineReason = command.DeclineReason,
                Remarks = command.Remarks
            };

            await TransferUserResponsibilityAsync(oldWorkflowResponsible, newWorkflowResponsible, command, token);
            document.WorkflowHistories.Add(newWorkflowResponsible);

            await MailSenderService.SendMail_OnDepartmentSupervisorRejectedDecision(document, token);
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistoryLog lastWorkFlowRecord)
        {
            if (!IsTransitionAllowed(lastWorkFlowRecord, allowedTransitionStatuses))
            {
                TransitionNotAllowed(command);
                return false;
            }

            if (!Enum.IsDefined(typeof(Decision), command.Decision))
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"No decision was specified!",
                    TranslationCode = "dms.decision.backend.update.validation.notSpecified",
                    Parameters = new object[] { command.Decision }
                });
                return false;
            }
            
            return true;
        }
    }
}
