using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Commands.Create
{
    public class CreateSpecificObjectiveHandler : ICommandHandler<CreateSpecificObjectiveCommand, ResultObject>
    {
        private readonly ISpecificObjectiveService _specificObjectiveService;
        private readonly ISpecificObjectiveFunctionaryService _specificObjectiveFunctionaryService;
        private readonly IUploadedFileService _uploadedFileService;

        public CreateSpecificObjectiveHandler(
            ISpecificObjectiveService specificObjectiveService,
            ISpecificObjectiveFunctionaryService specificObjectiveFunctionaryService,
            IUploadedFileService uploadedFileService)
        {
            _specificObjectiveService = specificObjectiveService;
            _specificObjectiveFunctionaryService = specificObjectiveFunctionaryService;
            _uploadedFileService = uploadedFileService;
        }
        public async Task<ResultObject> Handle(CreateSpecificObjectiveCommand request, CancellationToken cancellationToken)
        {
            var specificObjective = new SpecificObjective
            {
                DepartmentId = request.DepartmentId,
                GeneralObjectiveId = request.GeneralObjectiveId,
                Objective = new Objective
                {
                    Title = request.Title,
                    Details = request.Details,
                }
            };

            await _specificObjectiveService.AddAsync(specificObjective, cancellationToken);

            if (request.SpecificObjectiveFunctionaryIds != null)
                await _specificObjectiveFunctionaryService.AddRangeAsync(specificObjective.ObjectiveId, request.SpecificObjectiveFunctionaryIds, cancellationToken);

            if (request.UploadedFileIds.Any())
            {
                await _uploadedFileService.UpdateUploadedFilesWithTargetIdAsync(request.UploadedFileIds, specificObjective.Objective.Id, TargetEntity.Objective, cancellationToken);
            }

            return ResultObject.Created(specificObjective.ObjectiveId);
        }
    }
}
