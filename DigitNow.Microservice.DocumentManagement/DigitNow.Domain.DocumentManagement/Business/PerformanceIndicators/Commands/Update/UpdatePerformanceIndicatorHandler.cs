using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.PerformanceIndicators.Commands.Update
{
    public class UpdatePerformanceIndicatorHandler : ICommandHandler<UpdatePerformanceIndicatorCommand, ResultObject>
    {
        private readonly IPerformanceIndicatorService _performanceIndicatorService;
        private readonly IUploadedFileService _uploadedFileService;
        private readonly IPerformanceIndicatorFunctionaryService _performanceIndicatorFunctionaryService;
        private readonly IMapper _mapper;
        public UpdatePerformanceIndicatorHandler(
             IPerformanceIndicatorService performanceIndicatorService,
             IUploadedFileService uploadedFileService,
             IPerformanceIndicatorFunctionaryService performanceIndicatorFunctionaryService,
             Mapper mapper)
        {
            _performanceIndicatorService = performanceIndicatorService;
            _performanceIndicatorFunctionaryService = performanceIndicatorFunctionaryService;
            _uploadedFileService = uploadedFileService;
            _mapper = mapper;
        }
        public async Task<ResultObject> Handle(UpdatePerformanceIndicatorCommand request, CancellationToken cancellationToken)
        {
            var initialPerformanceIndicator = await _performanceIndicatorService.GetByIdQuery(request.Id)
                .Include(item => item.PerformanceIndicatorFunctionaries)
                    .FirstOrDefaultAsync(cancellationToken);

            if (initialPerformanceIndicator == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"The Performance indicator with id '{request.Id}' was not found.",
                    TranslationCode = "documentManagement.performance-indicator.update.validation.entityNotFound",
                    Parameters = new object[] { request.Id }
                });

            _mapper.Map(request, initialPerformanceIndicator);

            await _performanceIndicatorService.UpdateAsync(initialPerformanceIndicator, cancellationToken);

            if (request.PerformanceIndicatorFunctionariesIds != null)
            {
                await _performanceIndicatorFunctionaryService.UpdateRangeAsync(initialPerformanceIndicator.Id,
                    request.PerformanceIndicatorFunctionariesIds, cancellationToken);
            }

            if (request.UploadedFileIds.Any())
            {
                var uploadedFileMappings = await _uploadedFileService.GetUploadedFileMappingsByTargetIdAsync(
                     initialPerformanceIndicator.Id,
                     TargetEntity.ScimPerformanceIndicator,
                     cancellationToken);

                var uploadedFileIds = uploadedFileMappings.Select(item => item.UploadedFileId);

                await _uploadedFileService.UpdateUploadedFilesWithTargetIdAsync(request.UploadedFileIds.Except(uploadedFileIds), initialPerformanceIndicator.Id, TargetEntity.ScimPerformanceIndicator, cancellationToken);
            }

            return ResultObject.Created(initialPerformanceIndicator.Id);
        }
    }
}
