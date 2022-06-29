using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers._Interfaces;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers.Functionary
{
    public class FunctionaryAsksForOpinion : IWorkflowHandler
    {
        private int[] allowedTransitionStatuses = { (int)DocumentStatus.InWorkAllocated, (int)DocumentStatus.InWorkDelegated };

        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            // functionar solicita opinia unui sef de departament -> Solicitat_Opinie_Nerepatizat
            // Data este mandatory?

            if (!Validate(command))
            {
                return command;
            }

            //command.Status = DocumentStatus.OpinionRequestedUnallocated;
            //command.RecipientHasChanged = true;
            //command.RecipientType = UserRole.HeadOfDepartment;

            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command)
        {
            if (command.RecipientId <= 0)
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"No responsible for department with id {command.RecipientId} was found.",
                    TranslationCode = "dms.headOfdepartment.backend.update.validation.entityNotFound",
                    Parameters = new object[] { command.RecipientId }
                });
                return false;

            }

            if (command.OpinionRequestedUntil is not null && command.OpinionRequestedUntil < DateTime.Now )
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"The entered date is invalid!",
                    TranslationCode = "dms.date.backend.update.validation.invalidDate",
                    Parameters = new object[] { command.OpinionRequestedUntil }
                });
                return false;
            }

            return true;
        }
    }
}
