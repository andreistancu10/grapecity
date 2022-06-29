
using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers._Interfaces;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers.Functionary
{
    public class FunctionaryDeclines : BaseWorkflowProvider, IWorkflowHandler
    {
        private int[] allowedTransitionStatuses = { (int)DocumentStatus.InWorkAllocated, (int)DocumentStatus.InWorkDelegated, (int)DocumentStatus.OpinionRequestedAllocated };

        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var document = await GetDocumentById(command.DocumentId, token);

            if (!Validate(command))
            {
                return command;
            }

            //Daca status = Solicitat_Opinie_Alocat => statusul devine lucru_Alocat
            // Daca  Status curent In lucru_Alocat => Nou_Declinat_competenta

            //command.Status = DocumentStatus.NewDeclinedCompetence;
            //command.RecipientHasChanged = true;
            //command.RecipientType = UserRole.Functionary;
            
            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.DeclineReason))
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"The reason of decline was not specified.",
                    TranslationCode = "dms.declineReason.backend.update.validation.notSpecified",
                    Parameters = new object[] { command.DeclineReason }
                });
                return false;

            }

            return true;
        }
    }
}
