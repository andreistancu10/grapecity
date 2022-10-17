using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Standards.Commands.Update
{
    public class UpdateStandardHandler : ICommandHandler<UpdateStandardCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IUploadedFileService _uploadedFileService;
        private readonly IStandardService _standardService;
        private readonly IStandardFunctionaryService _standardFunctionaryService;

        public UpdateStandardHandler(
            DocumentManagementDbContext dbContext,
            IUploadedFileService uploadedFileService,
            IMapper mapper,
            IStandardService standardService,
            IStandardFunctionaryService standardFunctionaryService)
        {
            _dbContext = dbContext;
            _uploadedFileService = uploadedFileService;
            _standardService = standardService;
            _standardFunctionaryService = standardFunctionaryService;
        }
        public async Task<ResultObject> Handle(UpdateStandardCommand request, CancellationToken cancellationToken)
        {
            var initialStandard = await _dbContext.Standards.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (initialStandard == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"The standard with id '{request.Id}' was not found.",
                    TranslationCode = "documentManagement.standard.update.validation.entityNotFound",
                    Parameters = new object[] { request.Id }
                });

            var fileMappings = await _uploadedFileService.GetUploadedFileMappingsByTargetIdAsync(initialStandard.Id, TargetEntity.ScimStandard, cancellationToken);

            initialStandard.Title = request.Title;
            initialStandard.StateId = request.StateId;
            initialStandard.Activity = request.Activity;
            initialStandard.DepartmentId = request.DepartmentId;
            initialStandard.Deadline = request.Deadline;
            initialStandard.Observations = request.Observations;
            initialStandard.ModificationMotive = request.ModificationMotive;

            await _standardService.UpdateAsync(initialStandard, cancellationToken);

            if (request.StandardFunctionariesIds != null)
            {
                await _standardFunctionaryService.UpdateRangeAsync(initialStandard.Id,
                    request.StandardFunctionariesIds, cancellationToken);
            }

            if (request.UploadedFileIds.Any())
            {
                var uploadedFileIds = fileMappings.Select(item => item.UploadedFileId);

                await _uploadedFileService.UpdateUploadedFilesWithTargetIdAsync(request.UploadedFileIds.Except(uploadedFileIds), initialStandard.Id, TargetEntity.ScimStandard, cancellationToken).ConfigureAwait(false);
            }

            return ResultObject.Ok(initialStandard.Id);
        }
    }
}
