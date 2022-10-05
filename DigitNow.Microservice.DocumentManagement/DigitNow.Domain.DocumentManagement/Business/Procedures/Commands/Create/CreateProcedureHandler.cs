using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Procedures.Commands.Create
{
    public class CreateProcedureHandler : ICommandHandler<CreateProcedureCommand, ResultObject>
    {
        private readonly IProcedureService _procedureService;
        private readonly IUploadedFileService _uploadedFileService;
        private readonly IProcedureFunctionaryService _procedureFunctionaryService;
        private readonly IMapper _mapper;

        public CreateProcedureHandler(
            IProcedureService procedureService,
            IUploadedFileService uploadedFileService,
            IMapper mapper,
            IProcedureFunctionaryService procedureFunctionaryService)
        {
            _procedureService = procedureService;
            _uploadedFileService = uploadedFileService;
            _procedureFunctionaryService = procedureFunctionaryService;
            _mapper = mapper;
        }
        public async Task<ResultObject> Handle(CreateProcedureCommand request, CancellationToken cancellationToken)
        {
            var newProcedure = _mapper.Map<Procedure>(request);

            await _procedureService.AddAsync(newProcedure, cancellationToken);
            await _procedureFunctionaryService.AddRangeAsync(newProcedure.Id, request.ProcedureFunctionariesIds, cancellationToken);

            if (request.UploadedFileIds.Any())
            {
                await _uploadedFileService.UpdateUploadedFilesWithTargetIdAsync(
                    request.UploadedFileIds,
                    newProcedure.Id,
                    TargetEntity.ScimProcedure,
                    cancellationToken);
            }
            return ResultObject.Created(newProcedure.Id);
        }
    }
}