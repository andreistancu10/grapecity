using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Standards.Commands.Create
{
    public class CreateStandardHandler : ICommandHandler<CreateStandardCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IUploadedFileService _uploadedFileService;
        private readonly IMapper _mapper;
        private readonly IStandardService _standardService;
        private readonly IStandardFunctionaryService _standardFunctionaryService;

        public CreateStandardHandler(
            DocumentManagementDbContext dbContext,
            IUploadedFileService uploadedFileService,
            IMapper mapper,
            IStandardService standardService,
            IStandardFunctionaryService standardFunctionaryService)
        {
            _dbContext = dbContext;
            _uploadedFileService = uploadedFileService;
            _mapper = mapper;   
            _standardService = standardService;
            _standardFunctionaryService = standardFunctionaryService;
        }
        public async Task<ResultObject> Handle(CreateStandardCommand request, CancellationToken cancellationToken)
        {
            var newStandard = _mapper.Map<Standard>(request);
            await _standardService.CreateAsync(newStandard, cancellationToken);

            if (request.StandardFunctionariesIds.Any())
                await _standardFunctionaryService.AddRangeAsync(newStandard.Id, request.StandardFunctionariesIds, cancellationToken);

            if (request.UploadedFileIds.Any())
            {
                await _uploadedFileService.UpdateUploadedFilesWithTargetIdAsync(
                    request.UploadedFileIds,
                    newStandard.Id,
                    TargetEntity.ScimStandard,
                    cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultObject.Created(newStandard.Id);
        }
    }
}
