using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers._Interfaces;
using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers.Mayor;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Models
{
    public class Mayor : IWorkflowHandler
    {
        private enum ActionType { MakeDecision };

        private Dictionary<ActionType, IWorkflowHandler> actionStrategy
            = new Dictionary<ActionType, IWorkflowHandler>();

        public Mayor()
        {
            actionStrategy.Add(ActionType.MakeDecision, new MayorMakesDecision());
        }
        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var actionKey = (ActionType)command.ActionType;
            if (!actionStrategy.ContainsKey(actionKey))
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"No Action with id {command.ActionType} is possible for a Mayor.",
                    TranslationCode = "dms.actionNotAllowed.backend.update.validation.actionNotAllowed",
                    Parameters = new object[] { command.ActionType }
                });
                return command;
            }

            return await actionStrategy[actionKey].CreateWorkflowRecord(command, token);
        }
    }
}
