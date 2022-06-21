
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers._Interfaces
{
    public interface IWorkflowHandler
    {
        public Task<ICreateWorkflowHistoryCommand> UpdateStatusBasedOnWorkflowDecision(ICreateWorkflowHistoryCommand command, Data.IncomingDocuments.IncomingDocument document);
    }
}
