using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Commands.Create
{
    using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Factory;

    public class CreateWorkflowHistoryHandler : ICommandHandler<CreateWorkflowDecisionCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateWorkflowHistoryHandler(DocumentManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<ResultObject> Handle(CreateWorkflowDecisionCommand request, CancellationToken cancellationToken)
        {
            var recipientType = WorkflowInitiatorFactory.Create(request.InitiatorType);

            var createWorkflowHistoryCommand = await recipientType.CreateWorkflowRecord(request);

            if (createWorkflowHistoryCommand.Result != null)
                return createWorkflowHistoryCommand.Result;


            return ResultObject.Ok();
        }
    }
}
