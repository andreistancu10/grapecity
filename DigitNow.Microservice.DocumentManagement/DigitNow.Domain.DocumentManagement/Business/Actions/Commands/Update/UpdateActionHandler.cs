using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Actions.Commands.Update
{
    public class UpdateActionHandler : ICommandHandler<UpdateActionCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IUploadedFileService _uploadedFileService;
        private readonly IActionService _actionService;
        private readonly IActionFunctionaryService _actionFunctionaryService;

        public UpdateActionHandler(
            DocumentManagementDbContext dbContext, 
            IUploadedFileService uploadedFileService,
            IActionService actionService,
            IActionFunctionaryService actionFunctionaryService)
        {
            _dbContext = dbContext;
            _uploadedFileService = uploadedFileService;
            _actionService = actionService;
            _actionFunctionaryService = actionFunctionaryService;
        }
        public async Task<ResultObject> Handle(UpdateActionCommand request, CancellationToken cancellationToken)
        {
            var initialAction = await _dbContext.Actions.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (initialAction == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"The action with id '{request.Id}' was not found.",
                    TranslationCode = "documentManagement.action.update.validation.entityNotFound",
                    Parameters = new object[] { request.Id }
                });

            var fileMappings = await _uploadedFileService.GetUploadedFileMappingsByTargetIdAsync(initialAction.Id, TargetEntity.ScimAction, cancellationToken);

            initialAction.Title = request.Title;
            initialAction.State = request.StateId;
            initialAction.Details = request.Details;
            initialAction.ModificationMotive = request.ModificationMotive;

            await _actionService.UpdateAsync(initialAction, cancellationToken);

            if (request.ActionFunctionariesIds != null)
            {
                await _actionFunctionaryService.UpdateRangeAsync(initialAction.Id,
                    request.ActionFunctionariesIds, cancellationToken);
            }

            if (request.UploadedFileIds.Any())
            {
                var uploadedFileIds = fileMappings.Select(item => item.UploadedFileId);

                await _uploadedFileService.UpdateUploadedFilesWithTargetIdAsync(request.UploadedFileIds.Except(uploadedFileIds), initialAction.Id, TargetEntity.ScimAction, cancellationToken).ConfigureAwait(false);
            }

            return ResultObject.Ok(initialAction.Id);
        }
    }
}
