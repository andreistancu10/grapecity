using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.Actions.Mayor
{
    public class MayorMakesDecision : BaseWorkflowManager, IWorkflowHandler
    {
        public MayorMakesDecision(IServiceProvider serviceProvider) : base(serviceProvider) { }

        private int[] allowedTransitionStatuses = { (int)DocumentStatus.InWorkMayorReview };
        private enum Decision { Declined, Approved };
        private Document _document;

        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            _document = await WorkflowService.GetDocumentById(command.DocumentId, token);
            var lastWorkFlowRecord = WorkflowService.GetLastWorkflowRecord(_document);

            if (!Validate(command, lastWorkFlowRecord))
                return command;

            switch ((Decision)command.Decision)
            {
                case Decision.Declined:
                    MayorDeclined(command);
                    break;
                case Decision.Approved:
                    MayorApproved(command);
                    break;
                default:
                    break;
            }

            await WorkflowService.CommitChangesAsync(token);

            return command;
        }

        private void MayorApproved(ICreateWorkflowHistoryCommand command)
        {
            var oldWorkflowResponsible = WorkflowService.GetOldWorkflowResponsible(_document, x => x.RecipientType == (int)UserRole.Functionary);

            var newWorkflowResponsible = new WorkflowHistory();
            TransferResponsibility(oldWorkflowResponsible, newWorkflowResponsible, command);

            newWorkflowResponsible.Status = _document.Status = DocumentStatus.InWorkCountersignature;

            _document.IncomingDocument.WorkflowHistory.Add(newWorkflowResponsible);
        }

        private void MayorDeclined(ICreateWorkflowHistoryCommand command)
        {
            var oldWorkflowResponsible = WorkflowService.GetOldWorkflowResponsible(_document, x => x.RecipientType == (int)UserRole.HeadOfDepartment);

            var newWorkflowResponsible = new WorkflowHistory();
            TransferResponsibility(oldWorkflowResponsible, newWorkflowResponsible, command);

            newWorkflowResponsible.Status = _document.Status = DocumentStatus.InWorkMayorDeclined;

            _document.IncomingDocument.WorkflowHistory.Add(newWorkflowResponsible);
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistory lastWorkFlowRecord)
        {
            if (!WorkflowService.IsTransitionAllowed(lastWorkFlowRecord, allowedTransitionStatuses))
            {
                TransitionNotAllowed(command);
                return false;
            }

            if (command.Decision != (int)Decision.Approved || command.Decision != (int)Decision.Declined)
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
