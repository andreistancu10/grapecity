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
        protected override int[] allowedTransitionStatuses => new int[] { (int)DocumentStatus.InWorkAllocated, (int)DocumentStatus.InWorkDelegated, (int)DocumentStatus.New, (int)DocumentStatus.InWorkDeclined };
        
        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, VirtualDocument virtualDocument, WorkflowHistory lastWorkFlowRecord, CancellationToken token)
        {
            if (!Validate(command, lastWorkFlowRecord))
                return command;

            var headOfDepartment = await FetchHeadOfDepartmentByDepartmentIdAsync((long)command.RecipientId, token);

            if (!UserExists(headOfDepartment, command))
                return command;

            virtualDocument.WorkflowHistory.Add(WorkflowHistoryFactory
                .Create(RecipientType.HeadOfDepartment, headOfDepartment, DocumentStatus.OpinionRequestedUnallocated, string.Empty, command.Remarks, command.OpinionRequestedUntil));

            await SetStatusAndRecipientBasedOnWorkflowDecision(command.DocumentId, headOfDepartment.Id, DocumentStatus.OpinionRequestedUnallocated);
            
            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistory lastWorkFlowRecord)
        {
            if (command.RecipientId <= 0 || !IsTransitionAllowed(lastWorkFlowRecord, allowedTransitionStatuses))
            {
                TransitionNotAllowed(command);
                return false;
            }

            if (command.OpinionRequestedUntil.HasValue)
            {
                var comparer = DateTime.Compare(command.OpinionRequestedUntil.Value, DateTime.Today);
                if (comparer < 0)
                {
                    command.Result = ResultObject.Error(new ErrorMessage
                    {
                        Message = $"The entered date is invalid!",
                        TranslationCode = "dms.date.backend.update.validation.invalidDate",
                        Parameters = new object[] { command.OpinionRequestedUntil }
                    });
                    return false;
                }
            }
            return true;
        }
    }
}
