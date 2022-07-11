using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using HTSS.Platform.Core.CQRS;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.Commands.Create
{
    public class CreateWorkflowHistoryHandler : ICommandHandler<CreateWorkflowDecisionCommand, ResultObject>
    {
        private readonly IServiceProvider _provider;

        public CreateWorkflowHistoryHandler(IServiceProvider provider)
        {
            _provider = provider;
        }
        public async Task<ResultObject> Handle(CreateWorkflowDecisionCommand request, CancellationToken cancellationToken)
        {
            var workflowInitiatorFactory = new WorkflowInitiatorFactory(_provider);
            var recipientTypeHandler = workflowInitiatorFactory.Create(request.InitiatorType);

            var createWorkflowHistoryCommand = await recipientTypeHandler.CreateWorkflowRecord(request, cancellationToken);

            if (createWorkflowHistoryCommand.Result != null)
                return createWorkflowHistoryCommand.Result;

            return ResultObject.Ok();
        }
    }
}
