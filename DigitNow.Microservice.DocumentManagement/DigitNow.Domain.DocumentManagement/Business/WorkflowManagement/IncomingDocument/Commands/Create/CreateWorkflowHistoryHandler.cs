using HTSS.Platform.Core.CQRS;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.Factory;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.IncomingDocument.Commands.Create
{

    public class CreateWorkflowHistoryHandler : ICommandHandler<CreateWorkflowDecisionCommand, ResultObject>
    {
        public async Task<ResultObject> Handle(CreateWorkflowDecisionCommand request, CancellationToken cancellationToken)
        {
            var recipientType = WorkflowInitiatorFactory.Create(request.InitiatorType);

            var createWorkflowHistoryCommand = await recipientType.CreateWorkflowRecord(request, cancellationToken);

            if (createWorkflowHistoryCommand.Result != null)
                return createWorkflowHistoryCommand.Result;

            return ResultObject.Ok();
        }
    }
}
