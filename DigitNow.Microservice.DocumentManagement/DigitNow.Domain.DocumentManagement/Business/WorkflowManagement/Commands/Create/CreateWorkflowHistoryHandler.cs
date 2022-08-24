using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowManagement.Commands.Create
{
    public class CreateWorkflowHistoryHandler : ICommandHandler<CreateWorkflowDecisionCommand, ResultObject>
    {
        private readonly IDocumentService _documentService;
        private readonly IServiceProvider _provider;

        public CreateWorkflowHistoryHandler(IDocumentService documentService, IServiceProvider provider)
        {
            _documentService = documentService;
            _provider = provider;
        }
        public async Task<ResultObject> Handle(CreateWorkflowDecisionCommand request, CancellationToken cancellationToken)
        {
            var documentIsEditable = await _documentService.CheckDocumentPermissionsAsync(request.DocumentId, cancellationToken);

            if (!documentIsEditable)
            {
                throw new AccessViolationException("Access rights are not met for this resource!");
            }

            var workflowInitiatorFactory = new WorkflowInitiatorFactory(_provider);
            var recipientTypeHandler = workflowInitiatorFactory.Create(request.InitiatorType);

            var createWorkflowHistoryCommand = await recipientTypeHandler.CreateWorkflowRecord(request, cancellationToken);

            if (createWorkflowHistoryCommand.Result != null)
                return createWorkflowHistoryCommand.Result;

            return ResultObject.Ok();
        }
    }
}
