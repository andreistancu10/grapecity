using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers._Interfaces;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using System.Threading;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers.HeadOfDepartment
{
    public class HeadOfDepartmentAllocatesRequest : IWorkflowHandler
    {
        private int[] allowedTransitionStatuses = { (int)DocumentStatus.InWorkUnallocated, (int)DocumentStatus.OpinionRequestedUnallocated, (int)DocumentStatus.InWorkDelegatedUnallocated };
        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            // sef departament repartizeaza catre un functionar din subordine -> In lucru_Alocat


            if (!Validate(command))
            {
                return command;
            }

            //var lastWorkflowEntry = doc.WorkflowHistory.OrderByDescending(x => x.CreationDate).FirstOrDefault();

            //newWorkflowEntry.Status = lastWorkflowEntry.Status == (int)Status.opinionRequestedUnallocated 
            //    ? newWorkflowEntry.Status = (int)Status.opinionRequestedAllocated 
            //    : (int)Status.inWorkAllocated;

            //newWorkflowEntry.RecipientType = (int)UserRole.Functionary;

            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command)
        {
            if (command.RecipientId <= 0)
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"Functionary not specified!",
                    TranslationCode = "dms.functionary.backend.update.validation.notSpcified",
                    Parameters = new object[] { command.Resolution }
                });
                return false;
            }

            return true;
        }
    }
}
