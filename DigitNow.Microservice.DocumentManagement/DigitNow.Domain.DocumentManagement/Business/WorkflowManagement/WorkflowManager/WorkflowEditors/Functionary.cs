namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.WorkflowEditors
{
    using DigitNow.Domain.DocumentManagement.Business.Common.Services;
    using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.Functionary;
    using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
    using HTSS.Platform.Core.CQRS;
    using HTSS.Platform.Core.Errors;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class Functionary : IWorkflowHandler
    {
        private enum ActionType { AskForOpinion = 1, AsksForApproval = 2, Finalize = 3, Decline = 4, SendOpinion = 5  };

        private readonly Dictionary<ActionType, IWorkflowHandler> actionStrategy 
            = new Dictionary<ActionType, IWorkflowHandler>();

        public Functionary(IServiceProvider serviceProvider, IMailSenderService mailSenderService)
        {
            actionStrategy.Add(ActionType.Decline, new FunctionaryDeclines(serviceProvider));
            actionStrategy.Add(ActionType.AskForOpinion, new FunctionaryAsksForOpinion(serviceProvider));
            actionStrategy.Add(ActionType.Finalize, new FunctionaryFinalizes(serviceProvider));
            actionStrategy.Add(ActionType.AsksForApproval, new FunctionaryAsksForApproval(serviceProvider, mailSenderService));
            actionStrategy.Add(ActionType.SendOpinion, new FunctionarySendsOpinion(serviceProvider));
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
