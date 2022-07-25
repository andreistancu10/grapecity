﻿using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.HeadOfDepartment
{
    public class HeadOfDepartmentMakesDecision : BaseWorkflowManager, IWorkflowHandler
    {
        public HeadOfDepartmentMakesDecision(IServiceProvider serviceProvider) : base(serviceProvider) { }
        protected override int[] allowedTransitionStatuses => new int[] { (int)DocumentStatus.InWorkApprovalRequested };
        private enum Decision { Approved = 1, Declined = 2};

        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, VirtualDocument virtualDocument, WorkflowHistory lastWorkFlowRecord, CancellationToken token)
        {
            if (!Validate(command, lastWorkFlowRecord))
                return command;

            switch ((Decision)command.Decision)
            {
                case Decision.Declined:
                    await ApplicationDeclined(command, virtualDocument);
                    break;
                case Decision.Approved:
                    await ApplicationApproved(command, virtualDocument, token);
                    break;
                default:
                    break;
            }

            return command;
        }

        private async Task<ICreateWorkflowHistoryCommand> ApplicationApproved(ICreateWorkflowHistoryCommand command, VirtualDocument document, CancellationToken token)
        {
            var userResponse = await FetchMayorAsync(token);

            if (!UserExists(userResponse, command))
                return command;

            document.WorkflowHistory
                .Add(WorkflowHistoryFactory
                .Create(RecipientType.Mayor, userResponse, DocumentStatus.InWorkMayorReview, command.DeclineReason, command.Remarks));

            await SetStatusAndRecipientBasedOnWorkflowDecision(command.DocumentId, userResponse.Id, DocumentStatus.InWorkMayorReview);

            return command;
        }

        private async Task ApplicationDeclined(ICreateWorkflowHistoryCommand command, VirtualDocument document)
        {
            var oldWorkflowResponsible = GetOldWorkflowResponsible(document, x => x.RecipientType == RecipientType.Functionary.Id);

            var newWorkflowResponsible = new WorkflowHistory
            {
                Status = DocumentStatus.InWorkDeclined,
                DeclineReason = command.DeclineReason,
                Remarks = command.Remarks
            };

            await TransferResponsibility(oldWorkflowResponsible, newWorkflowResponsible, command);

            document.WorkflowHistory.Add(newWorkflowResponsible);
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistory lastWorkFlowRecord)
        {
            if (!IsTransitionAllowed(lastWorkFlowRecord, allowedTransitionStatuses))
            {
                TransitionNotAllowed(command);
                return false;
            }

            if (!Enum.IsDefined(typeof(Decision), command.Decision))
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"No decision was specified!",
                    TranslationCode = "dms.decision.backend.update.validation.notSpecified",
                    Parameters = new object[] { command.Decision }
                });
                return false;
            }
            
            return true;
        }
    }
}
