using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers._Interfaces;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers.Functionary
{
    public class FunctionaryAsksForApproval : IWorkflowHandler
    {
        private int[] allowedTransitionStatuses = { (int)Status.inWorkAllocated, (int)Status.inWorkDelegated, (int)Status.opinionRequestedAllocated };

        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command)
        {
            if (!Validate(command))
            {
                return command;
            }

            command.Status = Status.inWorkApprovalRequested;
            command.RecipientHasChanged = true;
            command.RecipientType = UserRole.HeadOfDepartment;

            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command)
        {
            if (command.Resolution <= 0)
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"Selected resolution is invalid!",
                    TranslationCode = "dms.resolution.backend.update.validation.resolutionInvalid",
                    Parameters = new object[] { command.Resolution }
                });
                return false;
            }

            return true;
        }
    }
}
