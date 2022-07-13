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
    public class FunctionaryAsksForApproval : BaseWorkflowManager, IWorkflowHandler
    {
        public FunctionaryAsksForApproval(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override int[] allowedTransitionStatuses => new int[] { (int)DocumentStatus.InWorkAllocated, (int)DocumentStatus.InWorkDelegated, (int)DocumentStatus.OpinionRequestedAllocated };

        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var virtualDocument = await GetVirtualDocumentWorkflowHistoryByIdAsync(command.DocumentId, token);
            var lastWorkFlowRecord = GetLastWorkflowRecord(virtualDocument);

            if (!Validate(command, lastWorkFlowRecord))
                return command;

            var oldWorkflowResponsible = GetOldWorkflowResponsible(virtualDocument, x => x.RecipientType == UserRole.HeadOfDepartment.Id);

            var newWorkflowResponsible = new WorkflowHistory();

            newWorkflowResponsible.Status = DocumentStatus.InWorkApprovalRequested;
            newWorkflowResponsible.Resolution = command.Resolution;

            TransferResponsibility(oldWorkflowResponsible, newWorkflowResponsible, command);
            virtualDocument.WorkflowHistory.Add(newWorkflowResponsible);

            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistory lastWorkFlowRecord)
        {
            if (!IsTransitionAllowed(lastWorkFlowRecord, allowedTransitionStatuses))
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
