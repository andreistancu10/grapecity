using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Commands.Update
{
    public class UpdateGeneralObjectiveHandler : ICommandHandler<UpdateGeneralObjectiveCommand, ResultObject>
    {
        private readonly IGeneralObjectiveService _generalObjectiveService;
        private readonly IUploadedFileService _uploadedFileService;

        public UpdateGeneralObjectiveHandler(IGeneralObjectiveService generalObjectiveService,
            IUploadedFileService uploadedFileService)
        {
            _generalObjectiveService = generalObjectiveService;
            _uploadedFileService = uploadedFileService;
        }
        public async Task<ResultObject> Handle(UpdateGeneralObjectiveCommand request, CancellationToken cancellationToken)
        {
            var initialGeneralObjective = await _generalObjectiveService.FindQuery().Where(item => item.ObjectiveId == request.ObjectiveId)
                .Include(item => item.Objective)
                .ThenInclude(item => item.ObjectiveUploadedFiles)
                .FirstOrDefaultAsync(cancellationToken);

            if (initialGeneralObjective == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"The objective with id '{request.ObjectiveId}' was not found.",
                    TranslationCode = "documentManagement.generalObjective.update.validation.entityNotFound",
                    Parameters = new object[] { request.ObjectiveId }
                });

            initialGeneralObjective.Objective.Title = request.Title;
            initialGeneralObjective.Objective.State = request.State;
            initialGeneralObjective.Objective.Details = request.Details;
            initialGeneralObjective.Objective.ModificationMotive = request.ModificationMotive;

            await _generalObjectiveService.UpdateAsync(initialGeneralObjective, cancellationToken);

            if (request.UploadedFileIds.Any())
            {
                var uploadedFileIds = initialGeneralObjective.Objective.ObjectiveUploadedFiles.Select(item => item.UploadedFileId);

                await _uploadedFileService.CreateObjectiveUploadedFilesAsync(request.UploadedFileIds.Except(uploadedFileIds), initialGeneralObjective.Objective, cancellationToken).ConfigureAwait(false);
            }

            return ResultObject.Ok(initialGeneralObjective.Id);
        }
    }
}
