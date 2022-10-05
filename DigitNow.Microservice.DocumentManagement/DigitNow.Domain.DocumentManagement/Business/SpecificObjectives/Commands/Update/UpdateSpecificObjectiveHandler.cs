using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;

namespace DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Commands.Update
{
    public class UpdateSpecificObjectiveHandler : ICommandHandler<UpdateSpecificObjectiveCommand, ResultObject>
    {
        private readonly ISpecificObjectiveService _specificObjectiveService;
        private readonly ISpecificObjectiveFunctionaryService _specificObjectiveFunctionaryService;
        private readonly IUploadedFileService _uploadedFileService;

        public UpdateSpecificObjectiveHandler(
            ISpecificObjectiveService specificObjectiveService,
            ISpecificObjectiveFunctionaryService specificObjectiveFunctionaryService,
            IUploadedFileService uploadedFileService)
        {
            _specificObjectiveService = specificObjectiveService;
            _specificObjectiveFunctionaryService = specificObjectiveFunctionaryService;
            _uploadedFileService = uploadedFileService;
        }

        public async Task<ResultObject> Handle(UpdateSpecificObjectiveCommand request, CancellationToken cancellationToken)
        {
            var initialSpecificObjective = await _specificObjectiveService.FindQuery()
                .Where(item => item.ObjectiveId == request.ObjectiveId)
                .Include(item => item.Objective)
                .Include(item => item.SpecificObjectiveFunctionaries)
                .FirstOrDefaultAsync(cancellationToken);

            if (initialSpecificObjective == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"The objective with id '{request.ObjectiveId}' was not found.",
                    TranslationCode = "documentManagement.specificObjective.update.validation.entityNotFound",
                    Parameters = new object[] { request.ObjectiveId }
                });

            var uploadedFileMappings = await _uploadedFileService.GetUploadedFileMappingsByTargetIdAsync(
                 initialSpecificObjective.ObjectiveId,
                 TargetEntity.SpecificObjective,
                 cancellationToken);

            initialSpecificObjective.Objective.Title = request.Title;
            initialSpecificObjective.Objective.Details = request.Details;
            initialSpecificObjective.Objective.ModificationMotive = request.ModificationMotive;
            initialSpecificObjective.Objective.StateId = request.StateId;

            await _specificObjectiveService.UpdateAsync(initialSpecificObjective, cancellationToken);

            if (request.SpecificObjectiveFunctionaryIds != null)
            {
                await _specificObjectiveFunctionaryService.UpdateRangeAsync(initialSpecificObjective.ObjectiveId,
                    request.SpecificObjectiveFunctionaryIds, cancellationToken);
            }

            if (request.UploadedFileIds.Any())
            {
                var uploadedFileIds = uploadedFileMappings.Select(item => item.UploadedFileId);

                await _uploadedFileService.UpdateUploadedFilesWithTargetIdAsync(request.UploadedFileIds.Except(uploadedFileIds), initialSpecificObjective.ObjectiveId, TargetEntity.SpecificObjective, cancellationToken);
            }

            return ResultObject.Created(initialSpecificObjective.ObjectiveId);
        }
    }
}
