using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers._Interfaces;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.Queries;
using System;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Models
{
    public class HeadOfDepartment : IWorkflowHandler
    {
        private enum ActionType { Allocate, Decline, MakeDecision };
        public HeadOfDepartment(IDocumentsQueryService documentQueryService)
        {
        }
        public async Task<ICreateWorkflowHistoryCommand> UpdateStatusBasedOnWorkflowDecision(ICreateWorkflowHistoryCommand command)
        {
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

        public Task<ICreateWorkflowHistoryCommand> UpdateStatusBasedOnWorkflowDecision(ICreateWorkflowHistoryCommand command, Data.IncomingDocuments.IncomingDocument document)
        {
            throw new NotImplementedException();
        }
    }
}
