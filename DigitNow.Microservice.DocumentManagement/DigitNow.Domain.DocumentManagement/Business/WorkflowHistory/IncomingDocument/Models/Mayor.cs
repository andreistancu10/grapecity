using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers._Interfaces;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.Queries;
using System;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Models
{
    public class Mayor : IWorkflowHandler
    {
        private enum ActionType { MakeDecision };

        public Mayor()
        {

        }
        public async Task<ICreateWorkflowHistoryCommand> UpdateStatusBasedOnWorkflowDecision(ICreateWorkflowHistoryCommand command)
        {
            var actionType = (ActionType)command.ActionType;

            switch (actionType)
            {
                case ActionType.MakeDecision:
                    return MayorMakesDecision(command);
                default:
                    break;
            }

            return command;
        }

        private ICreateWorkflowHistoryCommand MayorMakesDecision(ICreateWorkflowHistoryCommand command)
        {
            throw new NotImplementedException();
        }

        public Task<ICreateWorkflowHistoryCommand> UpdateStatusBasedOnWorkflowDecision(ICreateWorkflowHistoryCommand command, Data.IncomingDocuments.IncomingDocument document)
        {
            throw new NotImplementedException();
        }
    }
}
