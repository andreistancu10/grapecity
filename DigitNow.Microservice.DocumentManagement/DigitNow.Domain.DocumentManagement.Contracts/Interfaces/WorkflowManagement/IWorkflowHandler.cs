using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Contracts.Interfaces.WorkflowManagement
{
    public interface IWorkflowHandler
    {
        public Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token);
    }
}
