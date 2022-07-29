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
    public class HeadOfDepartmentAsksForApproval : BaseWorkflowManager, IWorkflowHandler
    {
        public HeadOfDepartmentAsksForApproval(IServiceProvider serviceProvider) : base(serviceProvider) { }
        protected override int[] allowedTransitionStatuses => new int[] { (int)DocumentStatus.New };

        protected async override Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, WorkflowHistoryLog lastWorkflowRecord, CancellationToken token)
        {
            if (!Validate(command, lastWorkflowRecord, document))
                return command;

            var userResponse = await IdentityService.FetchMayorAsync(token);

            if (!UserExists(userResponse, command))
                return command;

            document.WorkflowHistories
                .Add(WorkflowHistoryLogFactory
                .Create(document.Id, RecipientType.Mayor, userResponse, DocumentStatus.InWorkMayorReview, string.Empty, command.Remarks, null, command.Resolution));

            await SetStatusAndRecipientBasedOnWorkflowDecisionAsync(command.DocumentId, userResponse.Id, DocumentStatus.InWorkMayorReview, token);

            return command;

        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistoryLog lastWorkFlowRecord, Document document)
        {
            if (!IsTransitionAllowed(lastWorkFlowRecord, allowedTransitionStatuses) || document.DocumentType == DocumentType.Incoming)
            {
                TransitionNotAllowed(command);
                return false;
            }

            if (command.Resolution <= 0)
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"Resolution was not set!",
                    TranslationCode = "dms.resolution.backend.update.validation.notSet",
                    Parameters = new object[] { command.Resolution }
                });
                return false;
            }

            return true;
        }
    }
}
