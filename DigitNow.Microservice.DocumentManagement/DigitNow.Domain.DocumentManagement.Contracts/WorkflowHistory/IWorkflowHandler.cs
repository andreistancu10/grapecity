
namespace DigitNow.Domain.DocumentManagement.Contracts.WorkflowHistory
{
    public interface IWorkflowHandler
    {
        public ICreateWorkflowHistoryCommand UpdateStatusBasedOnWorkflowDecision(ICreateWorkflowHistoryCommand command);
    }
}

