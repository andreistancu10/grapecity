using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.Mayor;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.WorkflowEditors
{
    public class Mayor : IWorkflowHandler
    {
        private enum ActionType { MakeDecision = 1 };

        private readonly Dictionary<ActionType, IWorkflowHandler> actionStrategy = new();

        public Mayor(IServiceProvider serviceProvider)
        {
            actionStrategy.Add(ActionType.MakeDecision, new MayorMakesDecision(serviceProvider));
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
