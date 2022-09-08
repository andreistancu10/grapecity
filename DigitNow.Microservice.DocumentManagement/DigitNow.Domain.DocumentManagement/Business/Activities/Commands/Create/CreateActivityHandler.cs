using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Activities.Commands.Create
{
    public class CreateActivityHandler : ICommandHandler<CreateActivityCommand, ResultObject>
    {
        private readonly IActivityService _activityService;
        private readonly IUploadedFileService _uploadedFileService;
        private readonly IActivityFunctionaryService _activityFunctionaryService;

        public CreateActivityHandler(
            IActivityService activityService,
            IUploadedFileService uploadedFileService,
            IActivityFunctionaryService activityFunctionaryService)
        {
            _activityService = activityService;
            _uploadedFileService = uploadedFileService;
            _activityFunctionaryService = activityFunctionaryService;
        }
        public async Task<ResultObject> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
        {
            var activity = new Activity
            {
                GeneralObjectiveId = request.GeneralObjectiveId,
                SpecificObjectiveId = request.SpecificObjectiveId,
                DepartmentId = request.DepartmentId,
                Title = request.Title,
                Details = request.Details
            };

            await _activityService.AddAsync(activity, cancellationToken);

            if (request.ActivityFunctionaryIds != null)
                await _activityFunctionaryService.AddRangeAsync(activity.Id, request.ActivityFunctionaryIds, cancellationToken);

            if (request.UploadedFileIds.Any())
            {
                await _uploadedFileService.UpdateUploadedFilesWithTargetIdAsync(request.UploadedFileIds, activity.Id, TargetEntity.ScimActivity, cancellationToken);
            }

            return ResultObject.Created(activity.Id);
        }
    }
}
