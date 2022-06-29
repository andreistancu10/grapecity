using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers._Interfaces;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers.HeadOfDepartment
{
    public class HeadOfDepartmentDeclines : IWorkflowHandler
    {
        private int[] allowedTransitionStatuses = { (int)DocumentStatus.InWorkUnallocated, (int)DocumentStatus.OpinionRequestedUnallocated, (int)DocumentStatus.InWorkDelegatedUnallocated };
        
        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            if (!Validate(command))
            {
                return command;
            }

            // Solicitat_Opinie_Nerepatizat => In lucru_Alocat
            //In_lucru_Nereprtizat => Nou_Declina_Competenta

            //command.Status = DocumentStatus.InWorkAllocated;
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
