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

            switch (document.DocumentType)
            {
                case DocumentType.Incoming:
                     await CreateWorkflowForIncomingDocumentAsync(command, document, token);
                    break;
                case DocumentType.Internal:
                case DocumentType.Outgoing:
                    await CreateWorkflowForOutgoingAndInternalDocumentAsync(command, document, token);
                    break;
                default:
                    return command;
            }

            return command;
        }

        #endregion

        private async Task CreateWorkflowForIncomingDocumentAsync(ICreateWorkflowHistoryCommand command, Document document, CancellationToken token)
        {
            document.Status = DocumentStatus.Finalized;
            await PassDocumentToRegistry(document, command, token);
        }

        private async Task CreateWorkflowForOutgoingAndInternalDocumentAsync(ICreateWorkflowHistoryCommand command, Document document, CancellationToken token)
        {
            var departmentToReceiveDocument = await IdentityService.GetCurrentUserFirstDepartmentIdAsync(token);

            document.DestinationDepartmentId = departmentToReceiveDocument;
            document.RecipientId = await IdentityService.GetHeadOfDepartmentUserIdAsync(departmentToReceiveDocument, token);
            document.Status = DocumentStatus.Finalized;

            await PassDocumentToDepartment(document, command, token);
        }

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
