using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.Mayor
{
    public class MayorMakesDecision : BaseWorkflowManager, IWorkflowHandler
    {
        public MayorMakesDecision(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override int[] allowedTransitionStatuses => new int[] { (int)DocumentStatus.InWorkMayorReview };
        private enum Decision { Approved = 1, Declined = 2  };
        private VirtualDocument _virtualDocument;

        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, VirtualDocument virtualDocument, WorkflowHistory lastWorkFlowRecord, CancellationToken token)
        {
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

            return command;
        }

        private void MayorApproved(ICreateWorkflowHistoryCommand command)
        {
            var oldWorkflowResponsible = GetOldWorkflowResponsible(_virtualDocument, x => x.RecipientType == UserRole.Functionary.Id);

            var newWorkflowResponsible = new WorkflowHistory
            {
                Status = DocumentStatus.InWorkCountersignature,
                Remarks = command.Remarks
            };

            TransferResponsibility(oldWorkflowResponsible, newWorkflowResponsible, command);

            _virtualDocument.WorkflowHistory.Add(newWorkflowResponsible);
        }

        private void MayorDeclined(ICreateWorkflowHistoryCommand command)
        {
            var oldWorkflowResponsible = GetOldWorkflowResponsible(_virtualDocument, x => x.RecipientType == UserRole.HeadOfDepartment.Id);

            var newWorkflowResponsible = new WorkflowHistory
            {
                Status = DocumentStatus.InWorkMayorDeclined,
                Remarks = command.Remarks
            };

            TransferResponsibility(oldWorkflowResponsible, newWorkflowResponsible, command);

            _virtualDocument.WorkflowHistory.Add(newWorkflowResponsible);
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
