using DigitNow.Domain.DocumentManagement.Contracts.WorkflowHistory;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.Models
{
    public class Mayor : IWorkflowHandler
    {
        public Task<ICreateWorkflowHistoryCommand> UpdateStatusBasedOnWorkflowDecision(ICreateWorkflowHistoryCommand command)
        {
            throw new System.NotImplementedException();
        }
    }
}
