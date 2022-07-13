using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.HeadOfDepartment
{
    public class HeadOfDepartmentDeclines : BaseWorkflowManager, IWorkflowHandler
    {
        public HeadOfDepartmentDeclines(IServiceProvider serviceProvider) : base(serviceProvider) { }

        private ICreateWorkflowHistoryCommand _command;
        protected override int[] allowedTransitionStatuses => new int[] { (int)DocumentStatus.InWorkUnallocated, (int)DocumentStatus.OpinionRequestedUnallocated, (int)DocumentStatus.InWorkDelegatedUnallocated };
        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            _command = command;
            var virtualDocument = await GetVirtualDocumentWorkflowHistoryByIdAsync(command.DocumentId, token);
            var lastWorkFlowRecord = GetLastWorkflowRecord(virtualDocument);

            if (!Validate(command, lastWorkFlowRecord))
                return command;

            var newDocumentStatus = lastWorkFlowRecord.Status == DocumentStatus.OpinionRequestedUnallocated
                ? DocumentStatus.InWorkAllocated
                : DocumentStatus.NewDeclinedCompetence;

            await SetResponsibleBasedOnStatus(virtualDocument, newDocumentStatus, token);

            return command;
        }

        private async Task SetResponsibleBasedOnStatus(VirtualDocument document, DocumentStatus status, CancellationToken token)
        {
            if (status == DocumentStatus.InWorkAllocated)
                PassDocumentToFunctionary(document, _command);
            else
                await PassDocumentToRegistry(document, _command, token);
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistory lastWorkFlowRecord)
        {
            if (lastWorkFlowRecord == null || !allowedTransitionStatuses.Contains((int)lastWorkFlowRecord.Status))
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"Transition not allwed!",
                    TranslationCode = "dms.invalidState.backend.update.validation.invalidState",
                    Parameters = new object[] { command.Resolution }
                });
                return false;
            }

            if (string.IsNullOrWhiteSpace(command.DeclineReason))
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"The reason of decline was not specified.",
                    TranslationCode = "dms.declineReason.backend.update.validation.notSpecified",
                    Parameters = new object[] { command.DeclineReason }
                });
                return false;
            }

            return true;
        }
    }
}
