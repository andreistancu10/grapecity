using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers._Interfaces;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers.Functionary
{
    public class FunctionarySendsOpinion : IWorkflowHandler
    {
        private int[] allowedTransitionStatuses = { (int)DocumentStatus.OpinionRequestedAllocated };

        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            if (!Validate(command))
            {
                return command;
            }


            //command.Status = DocumentStatus.InWorkAllocated;
            //command.RecipientHasChanged = true;
            //command.RecipientType = UserRole.Functionary; // ajunge la functionarul care a solicitat opinia

            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Remarks))
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"No opinion was specified!",
                    TranslationCode = "dms.opinion.backend.update.validation.notSpecified",
                    Parameters = new object[] { command.DeclineReason }
                });
                return false;

            }
            return true;
        }
    }
}
