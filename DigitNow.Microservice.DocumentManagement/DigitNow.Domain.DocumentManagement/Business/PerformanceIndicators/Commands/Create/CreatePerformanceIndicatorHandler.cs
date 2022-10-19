using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.PerformanceIndicators.Commands.Create
{
    public class CreatePerformanceIndicatorHandler: ICommandHandler<CreatePerformanceIndicatorCommand, ResultObject>
    {
        private readonly IPerformanceIndicatorService _performanceIndicatorService;
        private readonly IUploadedFileService _uploadedFileService;
        private readonly IPerformanceIndicatorFunctionaryService _performanceIndicatorFunctionaryService;
        private readonly IMapper _mapper;

        public CreatePerformanceIndicatorHandler(
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

        public async Task<ResultObject> Handle(CreatePerformanceIndicatorCommand request, CancellationToken cancellationToken)
        {
            var newPerformanceIndicator = _mapper.Map<PerformanceIndicator>(request);

            await _performanceIndicatorService.AddAsync(newPerformanceIndicator, cancellationToken);
            await _performanceIndicatorFunctionaryService.AddRangeAsync(newPerformanceIndicator.Id, request.PerformanceIndicatorFunctionariesIds, cancellationToken);

            if (request.UploadedFileIds.Any())
            {
                await _uploadedFileService.UpdateUploadedFilesWithTargetIdAsync(
                    request.UploadedFileIds,
                    newPerformanceIndicator.Id,
                    TargetEntity.ScimPerformanceIndicator,
                    cancellationToken);
            }
            return ResultObject.Created(newPerformanceIndicator.Id);
        }
    }
}
