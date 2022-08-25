using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
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
        private readonly IUploadedFileService _uploadedFileService;

        public CreateGeneralObjectiveHandler(DocumentManagementDbContext dbContext,
            IGeneralObjectiveService generalObjectiveService,
            IUploadedFileService uploadedFileService)
        {
            _dbContext = dbContext;
            _generalObjectiveService = generalObjectiveService;
            _uploadedFileService = uploadedFileService;
        }
        public async Task<ResultObject> Handle(CreateGeneralObjectiveCommand request, CancellationToken cancellationToken)
        {

            var generalObjective = new GeneralObjective
            {
                Objective = new Objective()
                {
                    Title = request.Title,
                    Details = request.Details,
                }
            };

            await _generalObjectiveService.AddAsync(generalObjective, cancellationToken);
            if (request.UploadedFileIds.Any())
                await _uploadedFileService.CreateObjectiveUploadedFilesAsync(request.UploadedFileIds, generalObjective.Objective, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultObject.Created(generalObjective.ObjectiveId);
        }
    }
}
