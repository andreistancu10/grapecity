using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.Handlers._Interfaces;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System.Threading.Tasks;
using System.Threading;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.Handlers.HeadOfDepartment
{
    public class HeadOfDepartmentAllocatesRequest : BaseWorkflowManager, IWorkflowHandler
    {
        private int[] allowedTransitionStatuses = { (int)DocumentStatus.InWorkUnallocated, (int)DocumentStatus.OpinionRequestedUnallocated, (int)DocumentStatus.InWorkDelegatedUnallocated };
        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var document = await GetDocumentById(command.DocumentId, token);
            var lastWorkFlowRecord = GetLastWorkflowRecord(document);

            if (!Validate(command, lastWorkFlowRecord))
                return command;

            var user = await  GetFunctionaryByIdAsync((long)command.RecipientId, token);

            var newDocumentStatus = lastWorkFlowRecord.Status == (int)DocumentStatus.OpinionRequestedUnallocated
                ? DocumentStatus.OpinionRequestedAllocated
                : DocumentStatus.InWorkAllocated;

            document.IncomingDocument.WorkflowHistory
                .Add(WorkflowHistoryFactory
                .Create(document, UserRole.Functionary, user, newDocumentStatus));

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
