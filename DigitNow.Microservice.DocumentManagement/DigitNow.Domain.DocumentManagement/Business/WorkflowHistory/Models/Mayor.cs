using DigitNow.Domain.DocumentManagement.Contracts.WorkflowHistory;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.Models
{
    public class Mayor : IWorkflowHandler
    {
        public ICreateWorkflowHistoryCommand UpdateStatusBasedOnWorkflowDecision(ICreateWorkflowHistoryCommand command)
        {
            throw new System.NotImplementedException();
        }
    }
}
