using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities.Risks;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Risks.Commands.CreateRisk
{
    public class CreateRiskHandler : ICommandHandler<CreateRiskCommand, ResultObject>
    {
        private readonly IRiskService _riskService;
        private readonly IUploadedFileService _uploadedFileService;
        private readonly IRiskControlActionService _riskControlActionService;
        private readonly IMapper _mapper;

        public CreateRiskHandler(
            IRiskService riskService,
            IUploadedFileService uploadedFileService,
            IMapper mapper,
            IRiskControlActionService riskControlActionService)
        {
            _riskService = riskService;
            _uploadedFileService = uploadedFileService;
            _riskControlActionService = riskControlActionService;
            _mapper = mapper;
        }
        public async Task<ResultObject> Handle(CreateRiskCommand request, CancellationToken cancellationToken)
        {
            var risk = _mapper.Map<Risk>(request);

            await _riskService.AddAsync(risk, cancellationToken);

            if (request.RiskControlActions.Any())
                await _riskControlActionService.AddRangeAsync(request.RiskControlActions, risk, cancellationToken);

            if (request.UploadedFileIds.Any())
            {
                await _uploadedFileService.UpdateUploadedFilesWithTargetIdAsync(request.UploadedFileIds, risk.Id, TargetEntity.ScimRisk, cancellationToken);
            }

            return ResultObject.Created(risk.Id);
        }
    }
}
