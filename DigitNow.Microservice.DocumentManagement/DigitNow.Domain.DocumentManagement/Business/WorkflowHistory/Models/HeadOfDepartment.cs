using DigitNow.Domain.DocumentManagement.Contracts.WorkflowHistory;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.Queries;
using System;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.Models
{
    public class HeadOfDepartment : IWorkflowHandler
    {
        private readonly IDocumentsQueryService _documentQueryService;
        private IncomingDocument _incomingDocument;
        private enum ActionType { Allocate, Decline, MakeDecision };
        public HeadOfDepartment(IDocumentsQueryService documentQueryService)
        {
            _documentQueryService = documentQueryService;
        }
        public async Task<ICreateWorkflowHistoryCommand> UpdateStatusBasedOnWorkflowDecision(ICreateWorkflowHistoryCommand command)
        {
            _incomingDocument = await _documentQueryService.GetIncomingDocumentByRegistrationNumber(command.RegistrationNumber);
            var actionType = (ActionType)command.ActionType;

            switch (actionType)
            {
                case ActionType.Allocate:
                    return HeadOfDepartmentAllocates(command);
                case ActionType.Decline:
                    return HeadOfDepartmentDeclined(command);
                case ActionType.MakeDecision:
                    return HeadOfDepartmentDecides(command);
                default:
                    break;
            }

            return command;
        }

        private ICreateWorkflowHistoryCommand HeadOfDepartmentDecides(ICreateWorkflowHistoryCommand command)
        {
            throw new NotImplementedException();
        }

        private ICreateWorkflowHistoryCommand HeadOfDepartmentAllocates(ICreateWorkflowHistoryCommand command)
        {
            throw new NotImplementedException();
        }

        private ICreateWorkflowHistoryCommand HeadOfDepartmentDeclined(ICreateWorkflowHistoryCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
