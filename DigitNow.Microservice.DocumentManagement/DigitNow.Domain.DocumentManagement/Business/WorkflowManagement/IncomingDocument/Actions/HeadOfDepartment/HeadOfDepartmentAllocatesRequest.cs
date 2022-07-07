using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System.Threading.Tasks;
using System.Threading;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.Actions.HeadOfDepartment
{
    public class HeadOfDepartmentAllocatesRequest : BaseWorkflowManager, IWorkflowHandler
    {
        private int[] allowedTransitionStatuses = { (int)DocumentStatus.InWorkUnallocated, (int)DocumentStatus.OpinionRequestedUnallocated, (int)DocumentStatus.InWorkDelegatedUnallocated };
        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var document = await WorkflowService.GetDocumentById(command.DocumentId, token);
            var lastWorkFlowRecord = WorkflowService.GetLastWorkflowRecord(document);

            if (!Validate(command, lastWorkFlowRecord))
                return command;

            var user = await  GetUserByIdAsync((long)command.RecipientId, token);

            var newDocumentStatus = lastWorkFlowRecord.Status == DocumentStatus.OpinionRequestedUnallocated
                ? DocumentStatus.OpinionRequestedAllocated
                : DocumentStatus.InWorkAllocated;

            document.IncomingDocument.WorkflowHistory
                .Add(WorkflowHistoryFactory
                .Create(document, UserRole.Functionary, user, newDocumentStatus));

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
