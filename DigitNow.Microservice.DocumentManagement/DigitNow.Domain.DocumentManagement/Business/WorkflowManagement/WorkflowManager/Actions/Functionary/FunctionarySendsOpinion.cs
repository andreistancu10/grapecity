using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.Functionary
{
    public class FunctionarySendsOpinion : BaseWorkflowManager, IWorkflowHandler
    {
        public FunctionarySendsOpinion(IServiceProvider serviceProvider) : base(serviceProvider) { }

        private int[] allowedTransitionStatuses = { (int)DocumentStatus.OpinionRequestedAllocated };
        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var document = await WorkflowService.GetDocumentById(command.DocumentId, token);
            var oldWorkflowResponsible = WorkflowService.GetLastWorkflowRecord(document);
            
            if (!Validate(command, oldWorkflowResponsible))
                return command;

            var responsibleHeadOfDepartmentRecord = document.IncomingDocument.WorkflowHistory
                .Where(x => x.RecipientType == UserRole.HeadOfDepartment.Id && x.Status == DocumentStatus.OpinionRequestedUnallocated)
                .OrderByDescending(x => x.CreationDate)
                .FirstOrDefault();

            // TODO: we have to decide weather we pass the document to the functionary or to the head department
            //var responsibleFunctionaryRecord = document.IncomingDocument.WorkflowHistory
            //    .Where(x => x.RecipientType == UserRole.Functionary.Id && x.Status == DocumentStatus.InWorkAllocated)
            //    .OrderByDescending(x => x.CreationDate)
            //    .FirstOrDefault();

            var newWorkflowResponsible = new WorkflowHistory();
            TransferResponsibility(responsibleHeadOfDepartmentRecord, newWorkflowResponsible, command);

            newWorkflowResponsible.Status = document.Status = DocumentStatus.InWorkAllocated;
            newWorkflowResponsible.Remarks = command.Remarks;

            document.IncomingDocument.WorkflowHistory.Add(newWorkflowResponsible);
            await WorkflowService.CommitChangesAsync(token);

            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistory lastWorkFlowRecord)
        {
            if (string.IsNullOrWhiteSpace(command.Remarks) || !WorkflowService.IsTransitionAllowed(lastWorkFlowRecord, allowedTransitionStatuses))
            {
                TransitionNotAllowed(command);
                return false;
            }

            return true;
        }
    }
}
