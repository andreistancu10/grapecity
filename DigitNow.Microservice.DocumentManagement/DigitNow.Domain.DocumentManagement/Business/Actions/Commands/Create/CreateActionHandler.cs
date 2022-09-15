using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Actions.Commands.Create
{
    public class CreateActionHandler : ICommandHandler<CreateActionCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IUploadedFileService _uploadedFileService;
        private readonly IMapper _mapper;
        private readonly IActionService _actionService;
        private readonly IActionFunctionaryService _actionFunctionaryService;

        public CreateActionHandler(
            DocumentManagementDbContext dbContext, 
            IUploadedFileService uploadedFileService, 
            IMapper mapper,
            IActionService actionService,
            IActionFunctionaryService actionFunctionaryService)
        {
            _dbContext = dbContext;
            _uploadedFileService = uploadedFileService;
            _mapper = mapper;
            _actionService = actionService;
            _actionFunctionaryService = actionFunctionaryService;
        }
        public async Task<ResultObject> Handle(CreateActionCommand request, CancellationToken cancellationToken)
        {
            var newAction = _mapper.Map<Data.Entities.Action>(request);
            await _actionService.CreateAsync(newAction, cancellationToken);

            if (request.ActionFunctionariesIds.Any())
                await _actionFunctionaryService.AddRangeAsync(newAction.Id, request.ActionFunctionariesIds, cancellationToken);

            if (request.UploadedFileIds.Any())
            {
                await _uploadedFileService.UpdateUploadedFilesWithTargetIdAsync(
                    request.UploadedFileIds,
                    newAction.Id,
                    TargetEntity.ScimAction,
                    cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultObject.Created(newAction.Id);
        }
    }
}
