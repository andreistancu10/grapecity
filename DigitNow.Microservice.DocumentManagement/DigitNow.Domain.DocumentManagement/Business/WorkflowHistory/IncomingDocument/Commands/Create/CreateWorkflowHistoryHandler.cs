using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Commands.Create
{
    using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.IncomingDocument.Factory;
    using DigitNow.Domain.DocumentManagement.Data.WorkflowHistories;
    using Microsoft.EntityFrameworkCore;

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

            var incomingDocFromDb = await _dbContext.IncomingDocuments
                                                    .Include(doc => doc.WorkflowHistory)
                                                    .FirstOrDefaultAsync(doc => doc.RegistrationNumber == request.RegistrationNumber);

            var createWorkflowHistoryCommand = await recipientType.CreateWorkflowRecord(request);

            if (createWorkflowHistoryCommand.Result != null)
                return createWorkflowHistoryCommand.Result;

            var workflowHistoryEntry = _mapper.Map<WorkflowHistory>(createWorkflowHistoryCommand);

            incomingDocFromDb.WorkflowHistory.Add(workflowHistoryEntry);

            return ResultObject.Ok();
        }
    }
}
