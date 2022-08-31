using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;

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

            var user = await IdentityService.GetUserByIdAsync((long)command.RecipientId, token);

            if (!UserExists(user, command))
                return command;

            var newDocumentStatus = lastWorkflowRecord.DocumentStatus == DocumentStatus.OpinionRequestedUnallocated
                ? DocumentStatus.OpinionRequestedAllocated
                : DocumentStatus.InWorkAllocated;

            document.WorkflowHistories
                .Add(WorkflowHistoryLogFactory
                .Create(document, RecipientType.Functionary, user, newDocumentStatus, default, command.Remarks));

            await UpdateDocumentBasedOnWorkflowDecisionAsync(makeDocumentVisibleForDepartment: false, command.DocumentId, user.Id, newDocumentStatus, token);
            
            if(lastWorkflowRecord.DocumentStatus == DocumentStatus.OpinionRequestedUnallocated && newDocumentStatus == DocumentStatus.OpinionRequestedAllocated)
            {
                await MailSenderService.SendMail_OnSupervisorAssignedOpinionRequestToFunctionary(user, document, token);
            }
            if(document.DocumentType == DocumentType.Incoming && newDocumentStatus == DocumentStatus.InWorkAllocated) 
            {
                await MailSenderService.SendMail_OnIncomingDocDictributedToFunctionary(user, document, token);
            }

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
