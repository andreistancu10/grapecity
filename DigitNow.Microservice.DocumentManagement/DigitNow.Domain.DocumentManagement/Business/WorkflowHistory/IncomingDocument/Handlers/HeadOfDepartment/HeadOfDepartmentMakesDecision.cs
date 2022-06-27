using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers._Interfaces;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers.HeadOfDepartment
{
    public class HeadOfDepartmentMakesDecision : IWorkflowHandler
    {
        private int[] allowedTransitionStatuses = { (int)Status.inWorkApprovalRequested };

        private enum Decision { Declined, Approved };
        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command)
        {
            if (!Validate(command))
            {
                return command;
            }

            switch ((Decision)command.Decision)
            {
                case Decision.Declined:
                    ApplicationDeclined(command);
                    break;
                case Decision.Approved:
                    ApplicationApproved(command);
                    break;
                default:
                    break;
            }
            
            return command;
        }

        private void ApplicationApproved(ICreateWorkflowHistoryCommand command)
        {
            command.Status = Status.inWorkMayorReview;
            command.RecipientHasChanged = true;
            command.RecipientType = UserRole.Mayor;
        }

        private void ApplicationDeclined(ICreateWorkflowHistoryCommand command)
        {
            command.Status = Status.inWorkDeclined;
            command.RecipientHasChanged = true;
            command.RecipientType = UserRole.Functionary;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command)
        {
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
