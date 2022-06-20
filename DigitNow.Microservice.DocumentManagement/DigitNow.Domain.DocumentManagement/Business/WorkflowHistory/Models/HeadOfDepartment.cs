using DigitNow.Domain.DocumentManagement.Contracts.WorkflowHistory;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.Queries;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.Models
{
    public class HeadOfDepartment : IWorkflowHandler
    {
        private readonly IDocumentsQueryService _documentQueryService;

        public HeadOfDepartment(IDocumentsQueryService documentQueryService)
        {
            _documentQueryService = documentQueryService;
        }
        Task<ICreateWorkflowHistoryCommand> IWorkflowHandler.UpdateStatusBasedOnWorkflowDecision(ICreateWorkflowHistoryCommand command)
        {
            throw new System.NotImplementedException();
        }
    }
}
