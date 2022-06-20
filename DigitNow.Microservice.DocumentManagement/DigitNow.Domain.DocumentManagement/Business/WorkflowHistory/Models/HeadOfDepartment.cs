using DigitNow.Domain.DocumentManagement.Contracts.WorkflowHistory;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.Models
{
    public class HeadOfDepartment : IWorkflowHandler
    {
        Task<ICreateWorkflowHistoryCommand> IWorkflowHandler.UpdateStatusBasedOnWorkflowDecision(ICreateWorkflowHistoryCommand command)
        {
            throw new System.NotImplementedException();
        }
    }
}
