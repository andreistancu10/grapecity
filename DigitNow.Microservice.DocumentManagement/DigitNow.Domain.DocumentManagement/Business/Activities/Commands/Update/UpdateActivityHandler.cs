using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Activities.Commands.Update
{
    public class UpdateActivityHandler : ICommandHandler<UpdateActivityCommand, ResultObject>
    {
        private readonly IActivityService _activityService;
        private readonly IActivityFunctionaryService _activityFunctionaryService;
        private readonly IUploadedFileService _uploadedFileService;

        public UpdateActivityHandler(
            IActivityService activityService,
            IActivityFunctionaryService activityFunctionaryService,
            IUploadedFileService uploadedFileService)
        {
            _activityService = activityService;
            _activityFunctionaryService = activityFunctionaryService;
            _uploadedFileService = uploadedFileService;
        }

        public async Task<ResultObject> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
        {
            var initialActivity = await _activityService.FindQuery().Where(item => item.Id == request.Id)
                .Include(item => item.AssociatedGeneralObjective)
                .Include(item => item.AssociatedSpecificObjective)
                .Include(item => item.ActivityFunctionaries)
                    .FirstOrDefaultAsync(cancellationToken);

            if (initialActivity == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"The activity with id '{request.Id}' was not found.",
                    TranslationCode = "documentManagement.activity.update.validation.entityNotFound",
                    Parameters = new object[] { request.Id }
                });

            initialActivity.Title = request.Title;
            initialActivity.Details = request.Details;
            initialActivity.ModificationMotive = request.ModificationMotive;
            initialActivity.State = request.StateId;

            await _activityService.UpdateAsync(initialActivity, cancellationToken);

            if (request.ActivityFunctionaryIds != null)
            {
                await _activityFunctionaryService.UpdateRangeAsync(initialActivity.Id,
                    request.ActivityFunctionaryIds, cancellationToken);
            }

            if (request.UploadedFileIds.Any())
            {
                var uploadedFileMappings = await _uploadedFileService.GetUploadedFileMappingsByTargetIdAsync(
                     initialActivity.Id,
                     TargetEntity.ScimActivity,
                     cancellationToken);

                var uploadedFileIds = uploadedFileMappings.Select(item => item.UploadedFileId);

                await _uploadedFileService.UpdateUploadedFilesWithTargetIdAsync(request.UploadedFileIds.Except(uploadedFileIds), initialActivity.Id, TargetEntity.ScimActivity, cancellationToken);
            }

            return ResultObject.Created(initialActivity.Id);
        }
    }
}