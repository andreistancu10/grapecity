
//using AutoMapper;
//using DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.Factory;
//using DigitNow.Domain.DocumentManagement.Data;
//using HTSS.Platform.Core.CQRS;
//using System.Threading;
//using System.Threading.Tasks;

//namespace DigitNow.Domain.DocumentManagement.Business.WorkflowHistory.Commands.Create
//{
//    using DigitNow.Domain.DocumentManagement.Data.WorkflowHistories;
//    using Microsoft.EntityFrameworkCore;
//    using System;

//    public class CreateWorkflowHistoryHandler : ICommandHandler<CreateWorkflowDecisionCommand, ResultObject>
//    {
//        private readonly DocumentManagementDbContext _dbContext;
//        private readonly IMapper _mapper;

//        public CreateWorkflowHistoryHandler(DocumentManagementDbContext dbContext, IMapper mapper)
//        {
//            _dbContext = dbContext;
//            _mapper = mapper;
//        }
//        public async Task<ResultObject> Handle(CreateWorkflowDecisionCommand request, CancellationToken cancellationToken)
//        {
//            var initiatorType = WorkflowInitiatorFactory.Create(request.InitiatorType);
//            var updatedCommand = initiatorType.UpdateStatusBasedOnWorkflowDecision(request);
//            updatedCommand.CreationDate = DateTime.Now;

//            var incomingDocFromDb = await _dbContext.IncomingDocuments
//                                                    .Include(doc => doc.WorkflowHistory)
//                                                    .FirstOrDefaultAsync(doc => doc.Id == request.DocumentId);

//            var workflowHistoryEntry = _mapper.Map<WorkflowHistory>(updatedCommand);

//            incomingDocFromDb.WorkflowHistory.Add(workflowHistoryEntry);

//            return ResultObject.Ok();
//        }
//    }
//}
