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
        protected override int[] allowedTransitionStatuses => new int[] { (int)DocumentStatus.InWorkAllocated, (int)DocumentStatus.InWorkDelegated };
        
        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var virtualDocument = await GetVirtualDocumentWorkflowHistoryByIdAsync(command.DocumentId, token);
            var lastWorkFlowRecord = GetLastWorkflowRecord(virtualDocument);

            if (!Validate(command, lastWorkFlowRecord))
                return command;

            var headOfDepartment = await FetchHeadOfDepartmentByDepartmentId((long)command.RecipientId, token);

            if (!UserExists(headOfDepartment, command))
                return command;

            virtualDocument.WorkflowHistory.Add(WorkflowHistoryFactory
                .Create(UserRole.HeadOfDepartment, headOfDepartment, DocumentStatus.OpinionRequestedUnallocated, string.Empty, command.Remarks, command.OpinionRequestedUntil));

            SetStatusAndRecipientBasedOnWorkflowDecision(command.DocumentId, headOfDepartment.Id, DocumentStatus.OpinionRequestedUnallocated);
            
            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistory lastWorkFlowRecord)
        {
            if (command.RecipientId <= 0 || !IsTransitionAllowed(lastWorkFlowRecord, allowedTransitionStatuses))
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
