using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;

namespace DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Commands.Create
{
    public class CreateGeneralObjectiveHandler : ICommandHandler<CreateGeneralObjectiveCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IGeneralObjectiveService _generalObjectiveService;
        private readonly IObjectiveFileService _objectiveFileService;

        public CreateGeneralObjectiveHandler(DocumentManagementDbContext dbContext,
            IGeneralObjectiveService generalObjectiveService,
            IObjectiveFileService objectiveFileService)
        {
            _dbContext = dbContext;
            _generalObjectiveService = generalObjectiveService;
            _objectiveFileService = objectiveFileService;
        }
        public async Task<ResultObject> Handle(CreateGeneralObjectiveCommand request, CancellationToken cancellationToken)
        {

            var generalObjective = new GeneralObjective
            {
                Objective = new Objective
                {
                    Title = request.Title,
                    Details = request.Details,
                }
            };

            await _generalObjectiveService.AddAsync(generalObjective, cancellationToken);
            
            if (request.UploadedFileIds.Any())
            {
                await _objectiveFileService.UpdateUploadedFilesWithTargetIdAsync(request.UploadedFileIds,
                    generalObjective.Objective, cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultObject.Created(generalObjective.ObjectiveId);
        }
    }
}
