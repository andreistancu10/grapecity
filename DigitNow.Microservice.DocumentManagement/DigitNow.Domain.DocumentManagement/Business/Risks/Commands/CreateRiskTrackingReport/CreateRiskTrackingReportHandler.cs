using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Risks.Commands.CreateRiskTrackingReport
{
    public class CreateRiskTrackingReportHandler : ICommandHandler<CreateRiskTrackingReportCommand, ResultObject>
    {
        private readonly IMapper _mapper;
        private readonly IRiskTrackingReportService _riskTrackingReportService;
        private readonly IRiskService _riskService;
        private readonly IUploadedFileService _uploadedFileService;

        public CreateRiskTrackingReportHandler(IRiskTrackingReportService riskTrackingReportService, IRiskService riskService, IUploadedFileService uploadedFileService, IMapper mapper)
        {
            _mapper = mapper;
            _riskTrackingReportService = riskTrackingReportService;
            _uploadedFileService = uploadedFileService;
            _riskService = riskService;
        }

        public async Task<ResultObject> Handle(CreateRiskTrackingReportCommand command, CancellationToken cancellationToken)
        {
            var riskTrackingReport = _mapper.Map<RiskTrackingReport>(command);

            var foundRisk = await _riskService.GetByIdQuery(riskTrackingReport.RiskId)
                                              .FirstOrDefaultAsync(cancellationToken);

            if (foundRisk == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"The risk with id '{command.RiskId}' was not found.",
                    TranslationCode = "documentManagement.risk.update.validation.entityNotFound",
                    Parameters = new object[] { command.RiskId }
                });

            var createdRiskTrackingReport = await _riskTrackingReportService.CreateRiskTrackingReportAsync(riskTrackingReport, cancellationToken);

            if (command.UploadedFileIds.Any())
            {
                await _uploadedFileService.UpdateUploadedFilesWithTargetIdAsync(command.UploadedFileIds, createdRiskTrackingReport.Id, TargetEntity.ScimRiskTrackingReport, cancellationToken);
            }

            return ResultObject.Created(createdRiskTrackingReport.Id);
        }
    }
}
