using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.HeadOfDepartment
{
    public class HeadOfDepartmentAllocatesRequest : BaseWorkflowManager, IWorkflowHandler
    {
        public HeadOfDepartmentAllocatesRequest(IServiceProvider serviceProvider) : base(serviceProvider) { }
        protected override int[] allowedTransitionStatuses => new int[] 
        { 
            (int)DocumentStatus.InWorkUnallocated, 
            (int)DocumentStatus.OpinionRequestedUnallocated, 
            (int)DocumentStatus.InWorkDelegatedUnallocated 
        };

        #region [ IWorkflowHandler ]
        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, WorkflowHistoryLog lastWorkflowRecord, CancellationToken token)
        {
            if (!Validate(command, lastWorkflowRecord))
                return command;

            var user = await IdentityAdapterClient.GetUserByIdAsync((long)command.RecipientId, token);

            if (!UserExists(user, command))
                return command;

            var newDocumentStatus = lastWorkflowRecord.DocumentStatus == DocumentStatus.OpinionRequestedUnallocated
                ? DocumentStatus.OpinionRequestedAllocated
                : DocumentStatus.InWorkAllocated;

            document.WorkflowHistories
                .Add(WorkflowHistoryLogFactory
                .Create(document, RecipientType.Functionary, user, newDocumentStatus));

            await UpdateDocumentBasedOnWorkflowDecisionAsync(makeDocumentVisibleForDepartment: false, command.DocumentId, user.Id, newDocumentStatus, token);

            return command;
        }

        #endregion

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistoryLog lastWorkFlowRecord)
        {
            if (command.RecipientId <= 0 || !IsTransitionAllowed(lastWorkFlowRecord, allowedTransitionStatuses))
            {
                TransitionNotAllowed(command);
                return false;
            }
            return true;
        }
    }
}
