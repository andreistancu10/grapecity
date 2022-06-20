using DigitNow.Domain.DocumentManagement.Contracts.WorkflowHistory;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.Queries;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.Models
{
    public class Mayor : IWorkflowHandler
    {
        private readonly IDocumentsQueryService _documentQueryService;

        public Mayor(IDocumentsQueryService documentQueryService)
        {
            _documentQueryService = documentQueryService;
        }
        public Task<ICreateWorkflowHistoryCommand> UpdateStatusBasedOnWorkflowDecision(ICreateWorkflowHistoryCommand command)
        {
            throw new System.NotImplementedException();
        }
    }
}
