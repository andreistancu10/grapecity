
using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers._Interfaces;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Handlers.Functionary
{
    public class FunctionaryDeclines : IWorkflowHandler
    {
        public async Task<ICreateWorkflowHistoryCommand> UpdateStatusBasedOnWorkflowDecision(ICreateWorkflowHistoryCommand command, Data.IncomingDocuments.IncomingDocument document)
        {
            return command;
        }
    }
}
