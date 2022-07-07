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

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.Actions.Functionary
{
    public class FunctionaryAsksForOpinion : BaseWorkflowManager, IWorkflowHandler
    {
        private int[] allowedTransitionStatuses = { (int)DocumentStatus.InWorkAllocated, (int)DocumentStatus.InWorkDelegated };

        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var document = await WorkflowService.GetDocumentById(command.DocumentId, token);
            var lastWorkFlowRecord = WorkflowService.GetLastWorkflowRecord(document);

            if (!Validate(command, lastWorkFlowRecord))
                return command;

            var headOfDepartment = await GetUserByDepartmentIdAsync((long)command.RecipientId, token);

            document.IncomingDocument.WorkflowHistory
                .Add(WorkflowHistoryFactory
                .Create(document, UserRole.HeadOfDepartment, headOfDepartment, DocumentStatus.OpinionRequestedUnallocated, string.Empty, command.Remarks, command.OpinionRequestedUntil));

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
