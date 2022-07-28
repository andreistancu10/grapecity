using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.HeadOfDepartment
{
    public class HeadOfDepartmentFinalizes : BaseWorkflowManager, IWorkflowHandler
    {
        public HeadOfDepartmentFinalizes(IServiceProvider serviceProvider) 
            : base(serviceProvider) { }

        protected override int[] allowedTransitionStatuses => new int[] { (int)DocumentStatus.New };

        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, WorkflowHistoryLog lastWorkFlowRecord, CancellationToken token)
        {
            if (!Validate(command, lastWorkFlowRecord, document))
                return command;

            var departmentToReceiveDocument = document.WorkflowHistories
                    .Where(x => x.RecipientType == RecipientType.Department.Id)
                    .OrderBy(x => x.CreatedAt)
                    .First().RecipientId;

            document.DestinationDepartmentId = departmentToReceiveDocument;
            document.RecipientId = await IdentityService.GetHeadOfDepartmentUserIdAsync(departmentToReceiveDocument, token);
            document.Status = DocumentStatus.Finalized;

            var newWorkflowResponsible = new WorkflowHistoryLog
            {
                DocumentStatus = DocumentStatus.Finalized,
                Remarks = command.Remarks,
                RecipientType = RecipientType.Department.Id,
                RecipientId = departmentToReceiveDocument,
                RecipientName = $"Departamentul {departmentToReceiveDocument}!"
            };

            document.WorkflowHistories.Add(newWorkflowResponsible);

            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistoryLog lastWorkFlowRecord, Document document)
        {
            if (!IsTransitionAllowed(lastWorkFlowRecord, allowedTransitionStatuses) || document.DocumentType == DocumentType.Incoming)
            {
                TransitionNotAllowed(command);
                return false;
            }

            return true;
        }
    }
}
