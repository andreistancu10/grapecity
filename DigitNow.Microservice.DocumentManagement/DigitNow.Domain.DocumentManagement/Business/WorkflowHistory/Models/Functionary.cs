namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.Models
{
    using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
    using DigitNow.Domain.DocumentManagement.Contracts.WorkflowHistory;
    using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;
    using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.Queries;
    using System.Linq;
    using System.Threading.Tasks;

    public class Functionary : IWorkflowHandler
    {
        private readonly IDocumentsQueryService _documentService;
        private IncomingDocument _incomingDocument;
        private enum ActionType { Decline, AskForOpinion, Finalize, SendForApproval, SendOpinion };

        public Functionary(IDocumentsQueryService documentQueryService)
        {
            _documentService = documentQueryService;
        }

        public async Task<ICreateWorkflowHistoryCommand> UpdateStatusBasedOnWorkflowDecision(ICreateWorkflowHistoryCommand command)
        {
            _incomingDocument = await _documentService.GetIncomingDocumentByRegistrationNumber(command.RegistrationNumber);

            

            var actionType = (ActionType)command.ActionType;

            switch (actionType)
            {
                case ActionType.Decline:
                    return FunctionaryDeclined(command);
                case ActionType.AskForOpinion:
                    return FunctionaryAsksForOpinion(command);
                case ActionType.Finalize:
                    return FunctionaryFinalized(command);
                case ActionType.SendForApproval:
                    return FunctionarySendsForApproval(command);
                case ActionType.SendOpinion:
                    return FunctonarySendsOpinion(command);
                default:
                    break;
            }

            return command;
        }

        private ICreateWorkflowHistoryCommand FunctonarySendsOpinion(ICreateWorkflowHistoryCommand command)
        {
            command.RecipientType = (int)UserRole.HeadOfDepartment;
            command.Status = (int)Status.inWorkAllocated;
            return command;
        }

        private ICreateWorkflowHistoryCommand FunctionarySendsForApproval(ICreateWorkflowHistoryCommand command)
        {
            command.RecipientType = (int)UserRole.HeadOfDepartment;
            command.Status = (int)Status.inWorkApprovalRequested;
            return command;
        }

        private ICreateWorkflowHistoryCommand FunctionaryFinalized(ICreateWorkflowHistoryCommand command)
        {
            var x = _incomingDocument.WorkflowHistory.LastOrDefault();

            command.RecipientName = x.RecipientName;
            command.RecipientId = x.RecipientId;
            command.Status = (int)Status.finalized;

            return command;
        }

        private ICreateWorkflowHistoryCommand FunctionaryAsksForOpinion(ICreateWorkflowHistoryCommand command)
        {
            command.RecipientType = (int)UserRole.HeadOfDepartment;
            command.Status = (int)Status.opinionRequestedUnallocated;
            return command;
        }

        private ICreateWorkflowHistoryCommand FunctionaryDeclined(ICreateWorkflowHistoryCommand command)
        {
            var x = _incomingDocument.WorkflowHistory.FirstOrDefault();

            command.RecipientName = x.RecipientName;
            command.RecipientId = x.RecipientId;
            command.Status = (int)Status.newDeclinedCompetence;

            return command;
        }
    }
}
