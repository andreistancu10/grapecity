using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.Actions._Interfaces;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.Actions.Functionary
{
    public class FunctionaryAsksForApproval : BaseWorkflowManager, IWorkflowHandler
    {
        private int[] allowedTransitionStatuses = { (int)DocumentStatus.InWorkAllocated, (int)DocumentStatus.InWorkDelegated, (int)DocumentStatus.OpinionRequestedAllocated };
        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var document = await GetDocumentById(command.DocumentId, token);
            var lastWorkFlowRecord = GetLastWorkflowRecord(document);

            if (!Validate(command, lastWorkFlowRecord))
                return command;

            var responsibleHeadOfDepartmentRecord = document.IncomingDocument.WorkflowHistory
                .Where(x => x.RecipientType == (int)UserRole.HeadOfDepartment)
                .OrderByDescending(x => x.CreationDate)
                .FirstOrDefault();

            //Todo check if record is not null

            ResetWorkflowRecord(responsibleHeadOfDepartmentRecord);

            responsibleHeadOfDepartmentRecord.Status = (int)DocumentStatus.InWorkApprovalRequested;

            //Todo make column int
            responsibleHeadOfDepartmentRecord.Resolution = command.Resolution.ToString();

            document.IncomingDocument.WorkflowHistory.Add(responsibleHeadOfDepartmentRecord);
            await SaveDocument(token);

            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistory lastWorkFlowRecord)
        {
            if (!IsTransitionAllowed(command, lastWorkFlowRecord, allowedTransitionStatuses)) return false;

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
