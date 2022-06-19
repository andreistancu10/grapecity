

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.Models
{
    using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.Commands.Create;
    using DigitNow.Domain.DocumentManagement.Contracts.WorkflowHistory;
    using DigitNow.Domain.DocumentManagement.Data;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;

    public class Functionary : IWorkflowHandler
    {
        private readonly DocumentManagementDbContext _context;

        public enum ActionType { Approve, Decline, AskForOpinion, Finalize, SendForApproval, SendOpinion };
        public int ActionId { get; set; } = 1;

        public Functionary()
        {
        }
        public Functionary(DocumentManagementDbContext context)
        {
            _context = context;
        }

        public ICreateWorkflowHistoryCommand UpdateStatusBasedOnWorkflowDecision(ICreateWorkflowHistoryCommand command)
        {
            var incomingDocument = _context.IncomingDocuments.Include(x => x.WorkflowHistory).Where(x => x.RegistrationNumber == command.RegistrationNumber).ToListAsync();
            var actionType = (ActionType)ActionId;

            switch (actionType)
            {
                case ActionType.Approve:
                    break;
                case ActionType.Decline:
                    command.Status = 2;
                    break;
                case ActionType.AskForOpinion:
                    break;
                case ActionType.Finalize:
                    break;
                case ActionType.SendForApproval:
                    break;
                case ActionType.SendOpinion:
                    break;
                default:
                    break;
            }

            return new CreateWorkflowDecisionCommand();
        }
    }
}
