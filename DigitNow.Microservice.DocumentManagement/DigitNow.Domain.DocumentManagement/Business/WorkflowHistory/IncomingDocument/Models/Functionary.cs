namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Models
{
    using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
    using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers._Interfaces;
    using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers.Functionary;
    using DigitNow.Domain.DocumentManagement.Data.Entities;
    using HTSS.Platform.Core.CQRS;
    using HTSS.Platform.Core.Errors;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class Functionary : IWorkflowHandler
    {
        private enum ActionType { Decline, AskForOpinion, Finalize, AsksForApproval, SendOpinion };

        private Dictionary<ActionType, IWorkflowHandler> actionStrategy 
            = new Dictionary<ActionType, IWorkflowHandler>();

        public Functionary()
        {
            actionStrategy.Add(ActionType.Decline, new FunctionaryDeclines());
            actionStrategy.Add(ActionType.AskForOpinion, new FunctionaryAsksForOpinion());
            actionStrategy.Add(ActionType.Finalize, new FunctionaryFinalizes());
            actionStrategy.Add(ActionType.AsksForApproval, new FunctionaryAsksForApproval());
            actionStrategy.Add(ActionType.SendOpinion, new FunctionarySendsOpinion());
        }

        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var actionKey = (ActionType)command.ActionType;
            if (!actionStrategy.ContainsKey(actionKey))
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"No Action with id {command.ActionType} is possible for a Functionary.",
                    TranslationCode = "dms.actionNotAllowed.backend.update.validation.actionNotAllowed",
                    Parameters = new object[] { command.ActionType }
                });
                return command;
            }

            return await actionStrategy[actionKey].CreateWorkflowRecord(command, token);
        }
    }
}
