using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.Actions.Functionary
{
    public class FunctionaryAsksForApproval : BaseWorkflowManager, IWorkflowHandler
    {
        private int[] allowedTransitionStatuses = { (int)DocumentStatus.InWorkAllocated, (int)DocumentStatus.InWorkDelegated, (int)DocumentStatus.OpinionRequestedAllocated };
        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var document = await WorkflowService.GetDocumentById(command.DocumentId, token);
            var lastWorkFlowRecord = WorkflowService.GetLastWorkflowRecord(document);

            if (!Validate(command, lastWorkFlowRecord))
                return command;

            var oldWorkflowResponsible = WorkflowService.GetOldWorkflowResponsible(document, x => x.RecipientType == (int)UserRole.HeadOfDepartment);

            var newWorkflowResponsible = new WorkflowHistory();
            ResetWorkflowRecord(oldWorkflowResponsible, newWorkflowResponsible, command);

            newWorkflowResponsible.Status = DocumentStatus.InWorkApprovalRequested;
            newWorkflowResponsible.Resolution = command.Resolution;

            document.IncomingDocument.WorkflowHistory.Add(newWorkflowResponsible);
            await WorkflowService.CommitChangesAsync(token);

            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistory lastWorkFlowRecord)
        {
            if (!WorkflowService.IsTransitionAllowed(lastWorkFlowRecord, allowedTransitionStatuses))
            {
                TransitionNotAllowed(command);
                return false;
            }

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
