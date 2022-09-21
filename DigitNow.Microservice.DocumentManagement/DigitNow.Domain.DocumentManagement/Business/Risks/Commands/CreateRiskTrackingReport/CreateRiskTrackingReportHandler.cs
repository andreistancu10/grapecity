using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Risks;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Risks.Commands.CreateRiskTrackingReport
{
    public class CreateRiskTrackingReportHandler : ICommandHandler<CreateRiskTrackingReportCommand, ResultObject>
    {
        private readonly IMapper _mapper;
        private readonly DocumentManagementDbContext _dbContext;

        public CreateRiskTrackingReportHandler(DocumentManagementDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<ResultObject> Handle(CreateRiskTrackingReportCommand command, CancellationToken cancellationToken)
        {
            var riskTrackingReport = _mapper.Map<RiskTrackingReport>(command);

            var foundRisk = await _dbContext.Risks.FirstOrDefaultAsync(x => x.Id == riskTrackingReport.RiskId, cancellationToken);

            if (foundRisk == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"The risk with id '{command.RiskId}' was not found.",
                    TranslationCode = "documentManagement.risk.update.validation.entityNotFound",
                    Parameters = new object[] { command.RiskId }
                });

            await _dbContext.RiskTrackingReports.AddAsync(riskTrackingReport, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultObject.Created(riskTrackingReport.Id);
        }
    }
}
