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
        protected override int[] allowedTransitionStatuses => new int[] { (int)DocumentStatus.OpinionRequestedAllocated };

        protected override async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecordInternal(ICreateWorkflowHistoryCommand command, Document document, WorkflowHistoryLog lastWorkflowRecord, CancellationToken token)
        {
            if (!Validate(command, lastWorkflowRecord))
                return command;

            var oldWorkflowResponsible = document.WorkflowHistories
                .Where(x => (x.RecipientType == RecipientType.Functionary.Id && (x.DocumentStatus == DocumentStatus.InWorkAllocated || x.DocumentStatus == DocumentStatus.New)) 
                          || x.RecipientType == RecipientType.HeadOfDepartment.Id && x.DocumentStatus == DocumentStatus.OpinionRequestedUnallocated)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault();

            var newWorkflowResponsible = new WorkflowHistoryLog
            {
                DocumentStatus = document.DocumentType == DocumentType.Incoming ? DocumentStatus.InWorkAllocated : DocumentStatus.New,
                Remarks = command.Remarks
            };

            await TransferResponsibilityAsync(oldWorkflowResponsible, newWorkflowResponsible, command, token);

            document.WorkflowHistories.Add(newWorkflowResponsible);

            ResetDateAsOpinionWasSent(document);

            return command;
        }

        private static void ResetDateAsOpinionWasSent(Document document)
        {
            document.WorkflowHistories.ForEach(x => x.OpinionRequestedUntil = null);
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistoryLog lastWorkFlowRecord)
        {
            if (string.IsNullOrWhiteSpace(command.Remarks) || !IsTransitionAllowed(lastWorkFlowRecord, allowedTransitionStatuses))
            {
                TransitionNotAllowed(command);
                return false;
            }

            return true;
        }
    }
}
