using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.HeadOfDepartment
{
    public class HeadOfDepartmentFinalizes : BaseWorkflowManager, IWorkflowHandler
    {
        public HeadOfDepartmentFinalizes(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override int[] allowedTransitionStatuses => new int[] 
        { 
            (int)DocumentStatus.New,
            (int)DocumentStatus.InWorkUnallocated,
            (int)DocumentStatus.InWorkMayorDeclined
        };

        #region [ IWorkflowHandler ]
        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, WorkflowHistoryLog lastWorkflowRecord, CancellationToken token)
        {
            if (!Validate(command, lastWorkflowRecord, document))
                return command;

            var departmentToReceiveDocument = await IdentityService.GetCurrentUserFirstDepartmentAsync(token);

            document.DestinationDepartmentId = departmentToReceiveDocument.Id;
            document.RecipientId = await IdentityService.GetHeadOfDepartmentUserIdAsync(departmentToReceiveDocument.Id, token);
            document.Status = DocumentStatus.Finalized;

            await PassDocumentToDepartment(document, command, token);

            return command;
        }

        #endregion

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistoryLog lastWorkFlowRecord, Document document)
        {
            if (!IsTransitionAllowed(lastWorkFlowRecord, allowedTransitionStatuses))
            {
                TransitionNotAllowed(command);
                return false;
            }

            return true;
        }
    }
}
