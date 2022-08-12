using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.Commands.Create
{
    public class CreateWorkflowHistoryHandler : ICommandHandler<CreateWorkflowDecisionCommand, ResultObject>
    {
        private readonly IServiceProvider _provider;
        private readonly IMailSenderService _mailSenderService;

        public CreateWorkflowHistoryHandler(IServiceProvider provider, IMailSenderService mailSenderService)
        {
            _provider = provider;
            _mailSenderService = mailSenderService;
        }
        public async Task<ResultObject> Handle(CreateWorkflowDecisionCommand request, CancellationToken cancellationToken)
        {
            var workflowInitiatorFactory = new WorkflowInitiatorFactory(_provider, _mailSenderService);
            var recipientTypeHandler = workflowInitiatorFactory.Create(request.InitiatorType);

            var createWorkflowHistoryCommand = await recipientTypeHandler.CreateWorkflowRecord(request, cancellationToken);

            if (createWorkflowHistoryCommand.Result != null)
                return createWorkflowHistoryCommand.Result;

            return ResultObject.Ok();
        }
    }
}
