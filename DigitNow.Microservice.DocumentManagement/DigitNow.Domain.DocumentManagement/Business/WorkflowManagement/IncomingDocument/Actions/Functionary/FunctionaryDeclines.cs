
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.BaseManager;
using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.Actions._Interfaces;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.Actions.Functionary
{
    public class FunctionaryDeclines : BaseWorkflowManager, IWorkflowHandler
    {
        private int[] allowedTransitionStatuses = { (int)DocumentStatus.InWorkAllocated, (int)DocumentStatus.InWorkDelegated, (int)DocumentStatus.OpinionRequestedAllocated };
        private Document _document;
        private ICreateWorkflowHistoryCommand _command;
        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            _command = command;
            _document = await GetDocumentById(command.DocumentId, token);
            var lastWorkFlowRecord = GetLastWorkflowRecord(_document);

            if (!Validate(command, lastWorkFlowRecord))
                return command;

            var newDocumentStatus = lastWorkFlowRecord.Status == DocumentStatus.OpinionRequestedAllocated
                ? DocumentStatus.InWorkAllocated
                : DocumentStatus.NewDeclinedCompetence;

            if (newDocumentStatus == DocumentStatus.InWorkAllocated)
                PassDocumentToFunctionary();
            else
                await PassDocumentToRegistry(token);

            await SaveDocument(token);

            return command;
        }

        private async Task PassDocumentToRegistry(CancellationToken token)
        {
            var creator = await GetUserByIdAsync(_document.CreatedBy, token);

            _document.IncomingDocument.WorkflowHistory
                .Add(WorkflowHistoryFactory
                .Create(_document, UserRole.Functionary, creator, DocumentStatus.NewDeclinedCompetence, _command.DeclineReason, _command.Remarks));
        }

        private void PassDocumentToFunctionary()
        {
            var responsibleFunctionaryRecord = _document.IncomingDocument.WorkflowHistory
                .Where(x => x.RecipientType == (int)UserRole.Functionary)
                .OrderByDescending(x => x.CreationDate)
                .FirstOrDefault();

            ResetWorkflowRecord(responsibleFunctionaryRecord);

            responsibleFunctionaryRecord.Status = DocumentStatus.InWorkAllocated;
            responsibleFunctionaryRecord.DeclineReason = _command.DeclineReason;
            responsibleFunctionaryRecord.Remarks = _command.Remarks;

            _document.IncomingDocument.WorkflowHistory.Add(responsibleFunctionaryRecord);
        }

        private bool Validate(ICreateWorkflowHistoryCommand command, WorkflowHistory lastWorkFlowRecord)
        {
            if (!IsTransitionAllowed(command, lastWorkFlowRecord, allowedTransitionStatuses)) return false;

            if (string.IsNullOrWhiteSpace(command.DeclineReason))
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"The reason of decline was not specified.",
                    TranslationCode = "dms.declineReason.backend.update.validation.notSpecified",
                    Parameters = new object[] { command.DeclineReason }
                });
                return false;
            }
            return true;
        }
    }
}
