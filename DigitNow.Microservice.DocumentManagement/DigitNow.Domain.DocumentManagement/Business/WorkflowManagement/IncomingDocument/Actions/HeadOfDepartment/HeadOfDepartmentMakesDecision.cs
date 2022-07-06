using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.Actions.HeadOfDepartment
{
    public class HeadOfDepartmentMakesDecision : BaseWorkflowManager, IWorkflowHandler
    {
        private int[] allowedTransitionStatuses = { (int)DocumentStatus.InWorkApprovalRequested };
        private CancellationToken _token;
        private enum Decision { Approved = 1, Declined = 2};
        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            _token = token;
            var document = await GetDocumentById(command.DocumentId, token);
            var lastWorkFlowRecord = GetLastWorkflowRecord(document);

            if (!Validate(command, lastWorkFlowRecord))
                return command;

            switch ((Decision)command.Decision)
            {
                case Decision.Declined:
                    await ApplicationDeclined(command, document);
                    break;
                case Decision.Approved:
                    await ApplicationApproved(command, document);
                    break;
                default:
                    break;
            }
            
            return command;
        }

        private async Task<ICreateWorkflowHistoryCommand> ApplicationApproved(ICreateWorkflowHistoryCommand command, Document document)
        {
            var user = await GetMayorAsync(2, _token);

            if (!UserExists(user, command))
                return command;

            document.IncomingDocument.WorkflowHistory
                .Add(WorkflowHistoryFactory
                .Create(document, UserRole.Mayor, user, DocumentStatus.InWorkMayorReview, command.DeclineReason, command.Remarks));

            await SaveDocument(_token);

            return command;
        }

        private async Task<ICreateWorkflowHistoryCommand> ApplicationDeclined(ICreateWorkflowHistoryCommand command, Document document)
        {
            var responsibleFunctionaryRecord = document.IncomingDocument.WorkflowHistory
                .Where(x => x.RecipientType == (int)UserRole.Functionary)
                .OrderByDescending(x => x.CreationDate)
                .FirstOrDefault();

            ResetWorkflowRecord(responsibleFunctionaryRecord, command);

            responsibleFunctionaryRecord.Status = DocumentStatus.InWorkDeclined;
            responsibleFunctionaryRecord.DeclineReason = command.DeclineReason;
            responsibleFunctionaryRecord.Remarks = command.Remarks;


            document.IncomingDocument.WorkflowHistory.Add(responsibleFunctionaryRecord);
            await SaveDocument(_token);

            return command;
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistory lastWorkFlowRecord)
        {
            if (!IsTransitionAllowed(command, lastWorkFlowRecord, allowedTransitionStatuses)) return false;

            if (command.Decision != (int)Decision.Approved || command.Decision != (int)Decision.Declined)
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"No decision was specified!",
                    TranslationCode = "dms.decision.backend.update.validation.notSpecified",
                    Parameters = new object[] { command.Decision }
                });
                return false;
            }
            return true;
        }
    }
}
