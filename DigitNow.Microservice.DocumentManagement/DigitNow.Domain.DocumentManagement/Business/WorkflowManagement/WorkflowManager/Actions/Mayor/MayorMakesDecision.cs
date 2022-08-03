using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.Mayor
{
    public class MayorMakesDecision : BaseWorkflowManager, IWorkflowHandler
    {
        public MayorMakesDecision(IServiceProvider serviceProvider) : base(serviceProvider) { }
        private enum Decision { Approved = 1, Declined = 2 };

        protected override int[] allowedTransitionStatuses => new int[] 
        { 
            (int)DocumentStatus.InWorkMayorReview 
        };

        #region [ IWorkflowHandler ]
        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, WorkflowHistoryLog lastWorkflowRecord, CancellationToken token)
        {
            if (!Validate(command, lastWorkflowRecord))
                return command;

            switch ((Decision)command.Decision)
            {
                case Decision.Declined:
                    await MayorDeclinedAsync(command, document, token);
                    break;
                case Decision.Approved:
                    await MayorApprovedAsync(command, document, token);
                    break;
                default:
                    break;
            }

            return command;
        }

        #endregion

        private async Task MayorApprovedAsync(ICreateWorkflowHistoryCommand command, Document document, CancellationToken token)
        {
            switch (document.DocumentType)
            {
                case DocumentType.Incoming:
                    await MayorApprovedIncomingDocumentAsync(command, document, token);
                    break;
                case DocumentType.Internal:
                case DocumentType.Outgoing:
                    await MayorApprovedInternalOrOutgoingDocumentAsync(command, document, token);
                    break;
                default:
                    break;
            }
        }

        private async Task MayorApprovedInternalOrOutgoingDocumentAsync(ICreateWorkflowHistoryCommand command, Document document, CancellationToken token)
        {
            var departmentToReceiveDocument = document.WorkflowHistories
                .Where(x => x.RecipientType == RecipientType.Department.Id)
                .OrderBy(x => x.CreatedAt)
                .First().RecipientId;

            document.DestinationDepartmentId = departmentToReceiveDocument;
            document.RecipientId = await IdentityService.GetHeadOfDepartmentUserIdAsync(departmentToReceiveDocument, token);
            document.Status = DocumentStatus.InWorkCountersignature;

            await PassDocumentToDepartment(document, command, token);
        }

        private async Task MayorApprovedIncomingDocumentAsync(ICreateWorkflowHistoryCommand command, Document document, CancellationToken token)
        {
            var oldWorkflowResponsible = GetOldWorkflowResponsibleAsync(document, x => x.RecipientType == RecipientType.Functionary.Id);

            var newWorkflowResponsible = new WorkflowHistoryLog
            {
                DocumentStatus = DocumentStatus.InWorkCountersignature,
                Remarks = command.Remarks
            };

            await TransferUserResponsibilityAsync(oldWorkflowResponsible, newWorkflowResponsible, command, token);

            document.Status = DocumentStatus.InWorkCountersignature;
            document.WorkflowHistories.Add(newWorkflowResponsible);
        }

        private async Task MayorDeclinedAsync(ICreateWorkflowHistoryCommand command, Document document, CancellationToken token)
        {
            switch (document.DocumentType)
            {
                case DocumentType.Incoming:
                    await MayorDeclinedIncomingDocumentAsync(command, document, token);
                    break;
                case DocumentType.Internal:
                case DocumentType.Outgoing:
                    await MayorDeclinedInternalOrOutgoingDocumentAsync(command, document, token);
                    break;
                default:
                    break;
            }
        }

        private async Task MayorDeclinedInternalOrOutgoingDocumentAsync(ICreateWorkflowHistoryCommand command, Document document, CancellationToken token)
        {
            var departmentToReceiveDocument = document.WorkflowHistories
                .Where(x => x.RecipientType == RecipientType.Department.Id)
                .OrderBy(x => x.CreatedAt)
                .First().RecipientId;

            document.Status = DocumentStatus.InWorkMayorDeclined;
            document.DestinationDepartmentId = departmentToReceiveDocument;
            document.RecipientId = await IdentityService.GetHeadOfDepartmentUserIdAsync(departmentToReceiveDocument, token);

            await PassDocumentToDepartment(document, command, token);
        }

        private async Task MayorDeclinedIncomingDocumentAsync(ICreateWorkflowHistoryCommand command, Document document, CancellationToken token)
        {
            var oldWorkflowResponsible = GetOldWorkflowResponsibleAsync(document, x => x.RecipientType == RecipientType.HeadOfDepartment.Id);

            var newWorkflowResponsible = new WorkflowHistoryLog
            {
                DocumentStatus = DocumentStatus.InWorkMayorDeclined,
                Remarks = command.Remarks
            };

            await TransferUserResponsibilityAsync(oldWorkflowResponsible, newWorkflowResponsible, command, token);

            document.Status = DocumentStatus.InWorkMayorDeclined;
            document.WorkflowHistories.Add(newWorkflowResponsible);
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
