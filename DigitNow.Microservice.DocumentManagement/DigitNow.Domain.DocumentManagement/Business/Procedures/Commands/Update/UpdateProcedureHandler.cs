using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Procedures.Commands.Update
{
    public class UpdateProcedureHandler : ICommandHandler<UpdateProcedureCommand, ResultObject>
    {
        private readonly IProcedureService _procedureService;
        private readonly IProcedureFunctionaryService _procedureFunctionaryService;
        private readonly IProcedureHistoryService _procedureHistoryService;
        private readonly IUploadedFileService _uploadedFileService;
        private readonly IMapper _mapper;

        public UpdateProcedureHandler(
            IProcedureService procedureService,
            IProcedureFunctionaryService procedureFunctionaryService,
            IMapper mapper,
            IUploadedFileService uploadedFileService, 
            IProcedureHistoryService procedureHistoryService)
        {
            _procedureService = procedureService;
            _procedureFunctionaryService = procedureFunctionaryService;
            _uploadedFileService = uploadedFileService;
            _procedureHistoryService = procedureHistoryService;
            _mapper = mapper;
        }

        public async Task<ResultObject> Handle(UpdateProcedureCommand request, CancellationToken cancellationToken)
        {
            var initialProcedure = await _procedureService.GetByIdQuery(request.Id)
                .Include(item => item.ProcedureFunctionaries)
                    .FirstOrDefaultAsync(cancellationToken);

            if (initialProcedure == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"The Procedure with id '{request.Id}' was not found.",
                    TranslationCode = "documentManagement.procedure.update.validation.entityNotFound",
                    Parameters = new object[] { request.Id }
                });

            _mapper.Map(request, initialProcedure);

            await _procedureService.UpdateAsync(initialProcedure, cancellationToken);
            await _procedureHistoryService.AddAsync(initialProcedure,cancellationToken);

            if (request.ProcedureFunctionariesIds != null)
            {
                await _procedureFunctionaryService.UpdateRangeAsync(initialProcedure.Id,
                    request.ProcedureFunctionariesIds, cancellationToken);
            }

            if (request.UploadedFileIds.Any())
            {
                var uploadedFileMappings = await _uploadedFileService.GetUploadedFileMappingsByTargetIdAsync(
                     initialProcedure.Id,
                     TargetEntity.ScimProcedure,
                     cancellationToken);

                var uploadedFileIds = uploadedFileMappings.Select(item => item.UploadedFileId);

                await _uploadedFileService.UpdateUploadedFilesWithTargetIdAsync(request.UploadedFileIds.Except(uploadedFileIds), initialProcedure.Id, TargetEntity.ScimProcedure, cancellationToken);
            }

            return ResultObject.Created(initialProcedure.Id);
        }
    }
}
