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

        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, VirtualDocument virtualDocument, WorkflowHistory lastWorkFlowRecord, CancellationToken token)
        {
            if (!Validate(command, lastWorkFlowRecord))
                return command;

            switch ((Decision)command.Decision)
            {
                case Decision.Declined:
                    await MayorDeclined(command, virtualDocument, document);
                    break;
                case Decision.Approved:
                    await MayorApproved(command, virtualDocument, document);
                    break;
                default:
                    break;
            }

            return command;
        }

        private async Task MayorApproved(ICreateWorkflowHistoryCommand command, VirtualDocument virtualDocument, Document document)
        {
            switch (document.DocumentType)
            {
                case DocumentType.Incoming:
                    await MayorApprovedIncomingDocument(command, virtualDocument);
                    break;
                case DocumentType.Internal:
                case DocumentType.Outgoing:
                    MayorApprovedInternalOutgoingDocument(command, virtualDocument, document);
                    break;
                default:
                    break;
            }
        }

        private void MayorApprovedInternalOutgoingDocument(ICreateWorkflowHistoryCommand command, VirtualDocument virtualDocument, Document document)
        {
            var departmentToReceiveDocument = virtualDocument.WorkflowHistory
                .Where(x => x.RecipientType == RecipientType.Department.Id)
                .OrderBy(x => x.CreatedAt)
                .First().RecipientId;

            var newWorkflowResponsible = new WorkflowHistory
            {
                Status = DocumentStatus.InWorkCountersignature,
                Remarks = command.Remarks,
                RecipientType = RecipientType.Department.Id,
                RecipientId = departmentToReceiveDocument,
                RecipientName = $"Departamentul {departmentToReceiveDocument}!"
            };

            document.RecipientIsDepartment = true;
            document.RecipientId = departmentToReceiveDocument;

            virtualDocument.WorkflowHistory.Add(newWorkflowResponsible);
        }

        private async Task MayorApprovedIncomingDocument(ICreateWorkflowHistoryCommand command, VirtualDocument virtualDocument)
        {
            var oldWorkflowResponsible = GetOldWorkflowResponsible(virtualDocument, x => x.RecipientType == RecipientType.Functionary.Id);

            var newWorkflowResponsible = new WorkflowHistory
            {
                Status = DocumentStatus.InWorkCountersignature,
                Remarks = command.Remarks
            };

            await TransferResponsibility(oldWorkflowResponsible, newWorkflowResponsible, command);

            virtualDocument.WorkflowHistory.Add(newWorkflowResponsible);
        }

        private async Task MayorDeclined(ICreateWorkflowHistoryCommand command, VirtualDocument virtualDocument, Document document)
        {
            switch (document.DocumentType)
            {
                case DocumentType.Incoming:
                    await MayorDeclinedIncomingDocument(command, virtualDocument);
                    break;
                case DocumentType.Internal:
                case DocumentType.Outgoing:
                    MayorDeclinedIncomingDocumentInternalOutgoingDocument(command, virtualDocument, document);
                    break;
                default:
                    break;
            }

        }

        private void MayorDeclinedIncomingDocumentInternalOutgoingDocument(ICreateWorkflowHistoryCommand command, VirtualDocument virtualDocument, Document document)
        {
            var departmentToReceiveDocument = virtualDocument.WorkflowHistory
                .Where(x => x.RecipientType == RecipientType.Department.Id)
                .OrderBy(x => x.CreatedAt)
                .First().RecipientId;

            var newWorkflowResponsible = new WorkflowHistory
            {
                Status = DocumentStatus.InWorkMayorDeclined,
                Remarks = command.Remarks,
                RecipientType = RecipientType.Department.Id,
                RecipientId = departmentToReceiveDocument,
                RecipientName = $"Departamentul {departmentToReceiveDocument}!"
            };

            document.RecipientIsDepartment = true;
            document.RecipientId = departmentToReceiveDocument;

            virtualDocument.WorkflowHistory.Add(newWorkflowResponsible);
        }

        private async Task MayorDeclinedIncomingDocument(ICreateWorkflowHistoryCommand command, VirtualDocument virtualDocument)
        {
            var oldWorkflowResponsible = GetOldWorkflowResponsible(virtualDocument, x => x.RecipientType == RecipientType.HeadOfDepartment.Id);

            var newWorkflowResponsible = new WorkflowHistory
            {
                Status = DocumentStatus.InWorkMayorDeclined,
                Remarks = command.Remarks
            };

            await TransferResponsibility(oldWorkflowResponsible, newWorkflowResponsible, command);

            virtualDocument.WorkflowHistory.Add(newWorkflowResponsible);
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistory lastWorkFlowRecord)
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
