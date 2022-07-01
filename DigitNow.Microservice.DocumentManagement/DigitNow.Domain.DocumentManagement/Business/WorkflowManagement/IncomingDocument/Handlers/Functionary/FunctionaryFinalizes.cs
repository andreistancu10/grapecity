using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.Handlers._Interfaces;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.Handlers.Functionary
{
    public class FunctionaryFinalizes : IWorkflowHandler
    {
        private int[] allowedTransitionStatuses = { (int)DocumentStatus.InWorkAllocated, (int)DocumentStatus.InWorkDelegated };
        public async Task<ICreateWorkflowHistoryCommand> CreateWorkflowRecord(ICreateWorkflowHistoryCommand command, CancellationToken token)
        {
            //command.Status = DocumentStatus.Finalized;



            return command;
        }
    }
}
