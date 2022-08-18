using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Commands.Update
{
    public class UpdateSpecificObjectiveHandler : ICommandHandler<UpdateSpecificObjectiveCommand, ResultObject>
    {
        private readonly ISpecificObjectiveService _specificObjectiveService;
        private readonly IUploadedFileService _uploadedFileService;
        private readonly ISpecificObjectiveFunctionaryService _specificObjectiveFunctionaryService;

        public UpdateSpecificObjectiveHandler(ISpecificObjectiveService specificObjectiveService,
            ISpecificObjectiveFunctionaryService specificObjectiveFunctionaryService,
            IUploadedFileService uploadedFileService)
        {
            _specificObjectiveService = specificObjectiveService;
            _specificObjectiveFunctionaryService = specificObjectiveFunctionaryService;
            _uploadedFileService = uploadedFileService;
        }
        public async Task<ResultObject> Handle(UpdateSpecificObjectiveCommand request, CancellationToken cancellationToken)
        {
            var initialSpecificObjective = _specificObjectiveService.FindQuery(item => item.ObjectiveId == request.ObjectiveId,
                 new Expression<Func<SpecificObjective, object>>[] { item => item.Objective,
                     item => item.Objective.ObjectiveUploadedFiles,
                     item => item.SpecificObjectiveFunctionarys}).FirstOrDefault();

            if (initialSpecificObjective == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"The objective with id '{request.ObjectiveId}' was not found.",
                    TranslationCode = "documentManagement.specificObjective.update.validation.entityNotFound",
                    Parameters = new object[] { request.ObjectiveId }
                });

            initialSpecificObjective.Objective.Title = request.Title;
            initialSpecificObjective.Objective.Details = request.Details;
            initialSpecificObjective.Objective.ModificationMotive = request.ModificationMotive;
            initialSpecificObjective.Objective.State = request.State;

            await _specificObjectiveService.UpdateAsync(initialSpecificObjective, cancellationToken);

            if (request.SpecificObjectiveFunctionaryIds != null)
                await _specificObjectiveFunctionaryService.UpdateRangeAsync(initialSpecificObjective.ObjectiveId, request.SpecificObjectiveFunctionaryIds, cancellationToken);

            if (request.UploadedFileIds.Any())
            {
                var uploadedFileIds = initialSpecificObjective.Objective.ObjectiveUploadedFiles.Select(item => item.UploadedFileId);

                await _uploadedFileService.CreateObjectiveUploadedFilesAsync(request.UploadedFileIds.Except(uploadedFileIds), initialSpecificObjective.Objective, cancellationToken).ConfigureAwait(false);
            }

            return ResultObject.Created(initialSpecificObjective.Id);
        }
    }
}
