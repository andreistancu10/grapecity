
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Contracts.WorkflowHistory
{
    public interface IWorkflowHandler
    {
        public Task<ICreateWorkflowHistoryCommand> UpdateStatusBasedOnWorkflowDecision(ICreateWorkflowHistoryCommand command);
    }
}

