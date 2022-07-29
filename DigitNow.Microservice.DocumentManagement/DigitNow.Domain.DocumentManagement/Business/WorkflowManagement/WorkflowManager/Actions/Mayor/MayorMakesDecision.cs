using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.Documents;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.Mayor
{
    public class MayorMakesDecision : BaseWorkflowManager, IWorkflowHandler
    {
        public MayorMakesDecision(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override int[] allowedTransitionStatuses => new int[] { (int)DocumentStatus.InWorkMayorReview };
        private enum Decision { Approved = 1, Declined = 2  };

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

        private async Task MayorApprovedAsync(ICreateWorkflowHistoryCommand command, Document document, CancellationToken token)
        {
            switch (document.DocumentType)
            {
                case DocumentType.Incoming:
                    await MayorApprovedIncomingDocumentAsync(command, document, token);
                    break;
                case DocumentType.Internal:
                case DocumentType.Outgoing:
                    await MayorApprovedInternalOutgoingDocumentAsync(command, document, token);
                    break;
                default:
                    break;
            }
        }

        private async Task MayorApprovedInternalOutgoingDocumentAsync(ICreateWorkflowHistoryCommand command, Document document, CancellationToken token)
        {
            var departmentToReceiveDocument = document.WorkflowHistories
                .Where(x => x.RecipientType == RecipientType.Department.Id)
                .OrderBy(x => x.CreatedAt)
                .First().RecipientId;

            document.DestinationDepartmentId = departmentToReceiveDocument;
            document.RecipientId = await IdentityService.GetHeadOfDepartmentUserIdAsync(departmentToReceiveDocument, token);

            var newWorkflowResponsible = new WorkflowHistoryLog
            {
                DocumentStatus = DocumentStatus.InWorkCountersignature,
                Remarks = command.Remarks,
                RecipientType = RecipientType.Department.Id,
                RecipientId = departmentToReceiveDocument,
                RecipientName = $"Departamentul {departmentToReceiveDocument}!"
            };
            document.WorkflowHistories.Add(newWorkflowResponsible);
        }

        private async Task MayorApprovedIncomingDocumentAsync(ICreateWorkflowHistoryCommand command, Document document, CancellationToken token)
        {
            var oldWorkflowResponsible = await GetOldWorkflowResponsibleAsync(document, x => x.RecipientType == RecipientType.Functionary.Id, token);

            var newWorkflowResponsible = new WorkflowHistoryLog
            {
                DocumentStatus = DocumentStatus.InWorkCountersignature,
                Remarks = command.Remarks
            };

            await TransferResponsibilityAsync(oldWorkflowResponsible, newWorkflowResponsible, command, token);

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
                    await MayorDeclinedIncomingDocumentInternalOutgoingDocumentAsync(command, document, token);
                    break;
                default:
                    break;
            }

        }

        private async Task MayorDeclinedIncomingDocumentInternalOutgoingDocumentAsync(ICreateWorkflowHistoryCommand command, Document document, CancellationToken token)
        {
            var departmentToReceiveDocument = document.WorkflowHistories
                .Where(x => x.RecipientType == RecipientType.Department.Id)
                .OrderBy(x => x.CreatedAt)
                .First().RecipientId;

            var newWorkflowResponsible = new WorkflowHistoryLog
            {
                DocumentStatus = DocumentStatus.InWorkMayorDeclined,
                Remarks = command.Remarks,
                RecipientType = RecipientType.Department.Id,
                RecipientId = departmentToReceiveDocument,
                RecipientName = $"Departamentul {departmentToReceiveDocument}!"
            };

            document.DestinationDepartmentId = departmentToReceiveDocument;
            document.RecipientId = await IdentityService.GetHeadOfDepartmentUserIdAsync(departmentToReceiveDocument, token);

            document.WorkflowHistories.Add(newWorkflowResponsible);
        }

        private async Task MayorDeclinedIncomingDocumentAsync(ICreateWorkflowHistoryCommand command, Document document, CancellationToken token)
        {
            var oldWorkflowResponsible = await GetOldWorkflowResponsibleAsync(document, x => x.RecipientType == RecipientType.HeadOfDepartment.Id, token);

            var newWorkflowResponsible = new WorkflowHistoryLog
            {
                DocumentStatus = DocumentStatus.InWorkMayorDeclined,
                Remarks = command.Remarks
            };

            await TransferResponsibilityAsync(oldWorkflowResponsible, newWorkflowResponsible, command, token);

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
