using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.Actions.Mayor
{
    public class MayorMakesDecision : BaseWorkflowManager, IWorkflowHandler
    {
        private int[] allowedTransitionStatuses = { (int)DocumentStatus.InWorkMayorReview };
        private enum Decision { Declined, Approved };
        private Document _document;

        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            _document = await GetDocumentById(command.DocumentId, token);
            var lastWorkFlowRecord = GetLastWorkflowRecord(_document);

            if (!Validate(command, lastWorkFlowRecord))
                return command;

            switch ((Decision)command.Decision)
            {
                case Decision.Declined:
                    MayorDeclined(command);
                    break;
                case Decision.Approved:
                    MayorApproved(command);
                    break;
                default:
                    break;
            }

            await SaveDocument(token);

            return command;
        }

        private void MayorApproved(ICreateWorkflowHistoryCommand command)
        {
            var responsibleFunctionaryRecord = _document.IncomingDocument.WorkflowHistory
                .Where(x => x.RecipientType == (int)UserRole.Functionary)
                .OrderByDescending(x => x.CreationDate)
                .FirstOrDefault();

            ResetWorkflowRecord(responsibleFunctionaryRecord, command);

            responsibleFunctionaryRecord.Status = DocumentStatus.InWorkCountersignature;

            _document.IncomingDocument.WorkflowHistory.Add(responsibleFunctionaryRecord);
        }

        private void MayorDeclined(ICreateWorkflowHistoryCommand command)
        {
            var responsibleHeadOfDepartmentRecord = _document.IncomingDocument.WorkflowHistory
                .Where(x => x.RecipientType == (int)UserRole.HeadOfDepartment)
                .OrderByDescending(x => x.CreationDate)
                .FirstOrDefault();

            ResetWorkflowRecord(responsibleHeadOfDepartmentRecord, command);

            responsibleHeadOfDepartmentRecord.Status = DocumentStatus.InWorkMayorDeclined;

            _document.IncomingDocument.WorkflowHistory.Add(responsibleHeadOfDepartmentRecord);
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
