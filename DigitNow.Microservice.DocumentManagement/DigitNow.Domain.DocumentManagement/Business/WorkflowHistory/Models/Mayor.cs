using DigitNow.Domain.DocumentManagement.Contracts.WorkflowHistory;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.Queries;
using System;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.Models
{
    public class Mayor : IWorkflowHandler
    {
        private readonly IDocumentsQueryService _documentQueryService;
        private IncomingDocument _incomingDocument;
        private enum ActionType { MakeDecision };

        public Mayor(IDocumentsQueryService documentQueryService)
        {
            _documentQueryService = documentQueryService;
        }
        public async Task<ICreateWorkflowHistoryCommand> UpdateStatusBasedOnWorkflowDecision(ICreateWorkflowHistoryCommand command)
        {
            _incomingDocument = await _documentQueryService.GetIncomingDocumentByRegistrationNumber(command.RegistrationNumber);
            
            var actionType = (ActionType)command.ActionType;

            switch (actionType)
            {
                case ActionType.MakeDecision:
                    return MayorMakesDecision(command);
                default:
                    break;
            }

            return command;
        }

        private ICreateWorkflowHistoryCommand MayorMakesDecision(ICreateWorkflowHistoryCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
