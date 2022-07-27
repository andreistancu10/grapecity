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

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.Mayor
{
    public class MayorMakesDecision : BaseWorkflowManager, IWorkflowHandler
    {
        public MayorMakesDecision(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override int[] allowedTransitionStatuses => new int[] { (int)DocumentStatus.InWorkMayorReview };
        private enum Decision { Approved = 1, Declined = 2  };

        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, WorkflowHistoryLog lastWorkFlowRecord, CancellationToken token)
        {
            if (!Validate(command, lastWorkFlowRecord))
                return command;

            switch ((Decision)command.Decision)
            {
                case Decision.Declined:
                    await MayorDeclined(command, document);
                    break;
                case Decision.Approved:
                    await MayorApproved(command, document);
                    break;
                default:
                    break;
            }

            return command;
        }

        private async Task MayorApproved(ICreateWorkflowHistoryCommand command, Document document)
        {
            switch (document.DocumentType)
            {
                case DocumentType.Incoming:
                    await MayorApprovedIncomingDocument(command, document);
                    break;
                case DocumentType.Internal:
                case DocumentType.Outgoing:
                    MayorApprovedInternalOutgoingDocument(command, document);
                    break;
                default:
                    break;
            }
        }

        private void MayorApprovedInternalOutgoingDocument(ICreateWorkflowHistoryCommand command, Document document)
        {
            var departmentToReceiveDocument = document.WorkflowHistories
                .Where(x => x.RecipientType == RecipientType.Department.Id)
                .OrderBy(x => x.CreatedAt)
                .First().RecipientId;

            document.DestinationDepartmentId = departmentToReceiveDocument;
            document.RecipientId = null;

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

        private async Task MayorApprovedIncomingDocument(ICreateWorkflowHistoryCommand command, Document document)
        {
            var oldWorkflowResponsible = GetOldWorkflowResponsible(document, x => x.RecipientType == RecipientType.Functionary.Id);

            var newWorkflowResponsible = new WorkflowHistoryLog
            {
                DocumentStatus = DocumentStatus.InWorkCountersignature,
                Remarks = command.Remarks
            };

            await TransferResponsibility(oldWorkflowResponsible, newWorkflowResponsible, command);

            document.WorkflowHistories.Add(newWorkflowResponsible);
        }

        private async Task MayorDeclined(ICreateWorkflowHistoryCommand command, Document document)
        {
            switch (document.DocumentType)
            {
                case DocumentType.Incoming:
                    await MayorDeclinedIncomingDocument(command, document);
                    break;
                case DocumentType.Internal:
                case DocumentType.Outgoing:
                    MayorDeclinedIncomingDocumentInternalOutgoingDocument(command, document);
                    break;
                default:
                    break;
            }

        }

        private void MayorDeclinedIncomingDocumentInternalOutgoingDocument(ICreateWorkflowHistoryCommand command, Document document)
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
            document.RecipientId = null;

            document.WorkflowHistories.Add(newWorkflowResponsible);
        }

        private async Task MayorDeclinedIncomingDocument(ICreateWorkflowHistoryCommand command, Document document)
        {
            var oldWorkflowResponsible = GetOldWorkflowResponsible(document, x => x.RecipientType == RecipientType.HeadOfDepartment.Id);

            var newWorkflowResponsible = new WorkflowHistoryLog
            {
                DocumentStatus = DocumentStatus.InWorkMayorDeclined,
                Remarks = command.Remarks
            };

            await TransferResponsibility(oldWorkflowResponsible, newWorkflowResponsible, command);

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
