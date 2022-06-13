

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.Models
{
    using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.Commands.Create;
    using DigitNow.Domain.DocumentManagement.Contracts.WorkflowHistory;

    public class Functionary : IWorkflowHandler
    {
        public enum ActionType { Approve, Decline, AskForOpinion, Finalize, SendForApproval, SendOpinion };
        public int ActionId { get; set; } = 1;

        public ICreateWorkflowHistoryCommand UpdateStatusBasedOnWorkflowDecision(ICreateWorkflowHistoryCommand command)
        {
            var actionType = (ActionType)ActionId;

            switch (actionType)
            {
                case ActionType.Approve:
                    break;
                case ActionType.Decline:
                    command.Status = 2;
                    break;
                case ActionType.AskForOpinion:
                    break;
                case ActionType.Finalize:
                    break;
                case ActionType.SendForApproval:
                    break;
                case ActionType.SendOpinion:
                    break;
                default:
                    break;
            }

            return new CreateWorkflowDecisionCommand();
        }
    }
}
