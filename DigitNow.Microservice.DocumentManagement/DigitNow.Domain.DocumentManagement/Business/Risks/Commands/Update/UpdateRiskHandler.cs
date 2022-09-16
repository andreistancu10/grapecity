using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Risks.Commands.Update
{
    public class UpdateRiskHandler : ICommandHandler<UpdateRiskCommand, ResultObject>
    {
        private readonly IRiskService _riskService;
        private readonly IUploadedFileService _uploadedFileService;
        private readonly IRiskControlActionService _riskControlActionService;

        public UpdateRiskHandler(
            IRiskService riskService,
            IUploadedFileService uploadedFileService,
            IRiskControlActionService riskControlActionService)
        {
            _riskService = riskService;
            _uploadedFileService = uploadedFileService;
            _riskControlActionService = riskControlActionService;
        }
        public async Task<ResultObject> Handle(UpdateRiskCommand request, CancellationToken cancellationToken)
        {
            var initialRisk = await _riskService.FindQuery().Where(item => item.Id == request.Id)
                .Include(item => item.AssociatedGeneralObjective)
                .Include(item => item.AssociatedSpecificObjective)
                .Include(item => item.AssociatedActivity)
                .Include(item => item.AssociatedAction)
                .Include(item => item.RiskControlActions)
                    .FirstOrDefaultAsync(cancellationToken);

            if (initialRisk == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"The risk with id '{request.Id}' was not found.",
                    TranslationCode = "documentManagement.risk.update.validation.entityNotFound",
                    Parameters = new object[] { request.Id }
                });

            initialRisk.Description = request.Description;
            initialRisk.RiskCauses = request.RiskCauses;
            initialRisk.RiskConsequences = request.RiskConsequences;
            initialRisk.ProbabilityOfApparitionEstimation = request.ProbabilityOfApparitionEstimation;
            initialRisk.ImpactOfObjectivesEstimation = request.ImpactOfObjectivesEstimation;
            initialRisk.HeadOfDepartmentDecision = request.HeadOfDepartmentDecision;
            initialRisk.HeadOfDepartmentAssignation = request.HeadOfDepartmentAssignation;
            initialRisk.AdoptedStrategy = request.AdoptedStrategy;
            initialRisk.AdoptedStrategyAssignation = request.AdoptedStrategyAssignation;
            initialRisk.StrategyDetails = request.StrategyDetails;
            initialRisk.UtilizedDocumentation = request.UtilizedDocumentation;
            initialRisk.RiskExposureEvaluation =
                RiskService.CalculateRiskExposureEvaluation(request.ProbabilityOfApparitionEstimation, request.ImpactOfObjectivesEstimation);

            await _riskService.UpdateAsync(initialRisk, cancellationToken);

            if (request.RiskControlActions != null)
                await _riskControlActionService.AddRangeAsync(request.RiskControlActions, initialRisk, cancellationToken);

            if (request.UploadedFileIds.Any())
            {
                var uploadedFileMappings = await _uploadedFileService.GetUploadedFileMappingsByTargetIdAsync(
                      initialRisk.Id,
                      TargetEntity.ScimRisk,
                      cancellationToken);

                var uploadedFileIds = uploadedFileMappings.Select(item => item.UploadedFileId);

                await _uploadedFileService.UpdateUploadedFilesWithTargetIdAsync(request.UploadedFileIds.Except(uploadedFileIds), initialRisk.Id, TargetEntity.ScimRisk, cancellationToken);
            }

            return ResultObject.Created(initialRisk.Id);
        }
    }
}
