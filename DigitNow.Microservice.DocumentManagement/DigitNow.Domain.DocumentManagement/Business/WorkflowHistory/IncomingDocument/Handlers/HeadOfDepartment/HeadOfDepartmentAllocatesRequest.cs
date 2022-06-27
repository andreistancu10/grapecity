using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers._Interfaces;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers.HeadOfDepartment
{
    public class HeadOfDepartmentAllocatesRequest : IWorkflowHandler
    {
        private int[] allowedTransitionStatuses = { (int)Status.inWorkUnallocated, (int)Status.opinionRequestedUnallocated, (int)Status.inWorkDelegatedUnallocated };

        private Data.IncomingDocuments.IncomingDocument _doc;

        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowDecision(ICreateWorkflowHistoryCommand command, Data.IncomingDocuments.IncomingDocument doc)
        {
            _doc = doc;

            var newWorkflowEntry = new Data.WorkflowHistories.WorkflowHistory();
            // sef departament repartizeaza catre un functionar din subordine -> In lucru_Alocat

            if (!Validate(command))
                return command;

            var lastWorkflowEntry = doc.WorkflowHistory.OrderByDescending(x => x.CreationDate).FirstOrDefault();

            newWorkflowEntry.Status = lastWorkflowEntry.Status == (int)Status.opinionRequestedUnallocated 
                ? newWorkflowEntry.Status = (int)Status.opinionRequestedAllocated 
                : (int)Status.inWorkAllocated;

            newWorkflowEntry.RecipientType = (int)UserRole.Functionary;

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


        public Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
