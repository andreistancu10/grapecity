
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
using DigitNow.Domain.DocumentManagement.Data.Entities.Documents;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.HeadOfDepartment
{
    public class HeadOfDepartmentAsksForOpinion : BaseWorkflowManager, IWorkflowHandler
    {
        public HeadOfDepartmentAsksForOpinion(IServiceProvider serviceProvider) : base(serviceProvider) {}
        protected override int[] allowedTransitionStatuses => new int[] { (int)DocumentStatus.New };

        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, WorkflowHistoryLog lastWorkFlowRecord, CancellationToken token)
        {
            if (!Validate(command, lastWorkFlowRecord, document))
                return command;

            var headOfDepartment = await IdentityService.GetHeadOfDepartmentUserAsync((long)command.RecipientId, token);

            if (!UserExists(headOfDepartment, command))
                return command;

            document.WorkflowHistories.Add(WorkflowHistoryLogFactory
                .Create(document.Id, RecipientType.HeadOfDepartment, headOfDepartment, DocumentStatus.OpinionRequestedUnallocated, string.Empty, command.Remarks, command.OpinionRequestedUntil));

            await SetStatusAndRecipientBasedOnWorkflowDecisionAsync(command.DocumentId, headOfDepartment.Id, DocumentStatus.OpinionRequestedUnallocated, token);

            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistoryLog lastWorkFlowRecord, Document document)
        {
            if (command.RecipientId <= 0 || !IsTransitionAllowed(lastWorkFlowRecord, allowedTransitionStatuses) || document.DocumentType == DocumentType.Incoming)
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
