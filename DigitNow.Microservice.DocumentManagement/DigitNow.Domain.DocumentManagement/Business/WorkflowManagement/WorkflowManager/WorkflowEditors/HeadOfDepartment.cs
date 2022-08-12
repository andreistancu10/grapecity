using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.WorkflowManager.Actions.HeadOfDepartment;
using DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.WorkflowEditors
{
    public class HeadOfDepartment : IWorkflowHandler
    {
        private enum ActionType { Allocate = 1, Decline = 2, MakeDecision = 3, AsksForOpinion = 4, Finalizes = 5, AsksForApproval = 6 };

        private readonly Dictionary<ActionType, IWorkflowHandler> actionStrategy = new Dictionary<ActionType, IWorkflowHandler>();

        public HeadOfDepartment(IServiceProvider serviceProvider, IMailSenderService mailSenderService)
        {
            actionStrategy.Add(ActionType.Allocate, new HeadOfDepartmentAllocatesRequest(serviceProvider, mailSenderService));
            actionStrategy.Add(ActionType.Decline, new HeadOfDepartmentDeclines(serviceProvider));
            actionStrategy.Add(ActionType.MakeDecision, new HeadOfDepartmentMakesDecision(serviceProvider, mailSenderService));
            actionStrategy.Add(ActionType.AsksForOpinion, new HeadOfDepartmentAsksForOpinion(serviceProvider, mailSenderService));
            actionStrategy.Add(ActionType.Finalizes, new HeadOfDepartmentFinalizes(serviceProvider));
            actionStrategy.Add(ActionType.AsksForApproval, new HeadOfDepartmentAsksForApproval(serviceProvider));
        }

        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            var actionKey = (ActionType)command.ActionType;
            if (!actionStrategy.ContainsKey(actionKey))
            {
                command.Result = ResultObject.Error(new ErrorMessage
                {
                    Message = $"No Action with id {command.ActionType} is possible for a head of department.",
                    TranslationCode = "dms.actionNotAllowed.backend.update.validation.actionNotAllowed",
                    Parameters = new object[] { command.ActionType }
                });
                return command;
            }

            return await actionStrategy[actionKey].CreateWorkflowRecord(command, token);
        }
    }
}
