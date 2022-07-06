using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.Actions.Functionary
{
    public class FunctionarySendsOpinion : BaseWorkflowManager, IWorkflowHandler
    {
        private int[] allowedTransitionStatuses = { (int)DocumentStatus.OpinionRequestedAllocated };

        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var document = await GetDocumentById(command.DocumentId, token);
            var lastWorkFlowRecord = GetLastWorkflowRecord(document);
            
            if (!Validate(command, lastWorkFlowRecord))
                return command;

            var responsibleFunctionaryRecord = document.IncomingDocument.WorkflowHistory
                .Where(x => x.RecipientType == (int)UserRole.Functionary && x.Status == DocumentStatus.OpinionRequestedUnallocated)
                .OrderByDescending(x => x.CreationDate)
                .FirstOrDefault();

            ResetWorkflowRecord(responsibleFunctionaryRecord, command);

            responsibleFunctionaryRecord.Status = DocumentStatus.InWorkAllocated;
            responsibleFunctionaryRecord.Remarks = command.Remarks;

            document.IncomingDocument.WorkflowHistory.Add(responsibleFunctionaryRecord);

            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistory lastWorkFlowRecord)
        {
            if (!IsTransitionAllowed(command, lastWorkFlowRecord, allowedTransitionStatuses)) return false;

            if (string.IsNullOrWhiteSpace(command.Remarks))
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"No opinion was specified!",
                    TranslationCode = "dms.opinion.backend.update.validation.notSpecified",
                    Parameters = new object[] { command.DeclineReason }
                });
                return false;
            }
            return true;
        }
    }
}
