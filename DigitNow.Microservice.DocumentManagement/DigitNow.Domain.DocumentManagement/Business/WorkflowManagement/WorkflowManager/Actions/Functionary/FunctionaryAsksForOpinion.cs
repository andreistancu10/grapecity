using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.Functionary
{
    public class FunctionaryAsksForOpinion : BaseWorkflowManager, IWorkflowHandler
    {
        public FunctionaryAsksForOpinion(IServiceProvider serviceProvider) : base(serviceProvider) { }

        private int[] allowedTransitionStatuses = { (int)DocumentStatus.InWorkAllocated, (int)DocumentStatus.InWorkDelegated };
        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var document = await WorkflowService.GetDocumentById(command.DocumentId, token);
            var lastWorkFlowRecord = WorkflowService.GetLastWorkflowRecord(document);

            if (!Validate(command, lastWorkFlowRecord))
                return command;

            var headOfDepartment = await FetchHeadOfDepartmentByDepartmentId((long)command.RecipientId, token);

            if (!UserExists(headOfDepartment, command))
                return command;

            document.IncomingDocument.WorkflowHistory
                .Add(WorkflowHistoryFactory
                .Create(UserRole.HeadOfDepartment, headOfDepartment, DocumentStatus.OpinionRequestedUnallocated, string.Empty, command.Remarks, command.OpinionRequestedUntil));

            document.Status = DocumentStatus.OpinionRequestedUnallocated;
            
            await WorkflowService.CommitChangesAsync(token);

            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistory lastWorkFlowRecord)
        {
            if (command.RecipientId <= 0 || !WorkflowService.IsTransitionAllowed(lastWorkFlowRecord, allowedTransitionStatuses))
            {
                TransitionNotAllowed(command);
                return false;
            }

            if (command.OpinionRequestedUntil == null || command.OpinionRequestedUntil < DateTime.Now )
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
