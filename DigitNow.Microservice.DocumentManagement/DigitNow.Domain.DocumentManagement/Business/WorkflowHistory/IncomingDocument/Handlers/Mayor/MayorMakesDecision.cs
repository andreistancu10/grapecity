using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers._Interfaces;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers.Mayor
{
    public class MayorMakesDecision : IWorkflowHandler
    {
        private int[] allowedTransitionStatuses = { (int)Status.inWorkMayorReview };

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
            command.Status = Status.inWorkCountersignature;
            command.RecipientHasChanged = true;
            command.RecipientType = UserRole.Functionary;
        }

        private void MayorDeclined(ICreateWorkflowHistoryCommand command)
        {
            command.Status = Status.inWorkMayorDeclined;
            command.RecipientHasChanged = true;
            command.RecipientType = UserRole.HeadOfDepartment;
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
