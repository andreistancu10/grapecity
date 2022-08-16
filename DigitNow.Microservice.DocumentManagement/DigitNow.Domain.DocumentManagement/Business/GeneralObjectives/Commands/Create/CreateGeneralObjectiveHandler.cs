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
        private readonly IMapper _mapper;
        private readonly IUploadedFileService _uploadedFileService;

        public CreateGeneralObjectiveHandler(DocumentManagementDbContext dbContext, IGeneralObjectiveService generalObjectiveService)
        {
            _dbContext = dbContext;
            _generalObjectiveService = generalObjectiveService;
        }
        public async Task<ResultObject> Handle(CreateGeneralObjectiveCommand request, CancellationToken cancellationToken)
        {

            var generalObjective = new GeneralObjective
            {
                Objective = new Objective()
                {
                    Title = request.Title,
                    Details = request.Details,
                    ModificationMotive = request.ModificationMotive,
                }
            };

            await _generalObjectiveService.AddAsync(generalObjective, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultObject.Created(generalObjective.Id);
        }
    }
}
