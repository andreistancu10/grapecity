using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
using DigitNow.Domain.DocumentManagement.Business.Risks.Commands.CreateRiskTrackingReport;
using DigitNow.Domain.DocumentManagement.Data.Entities.Risks;

namespace DigitNow.Domain.DocumentManagement.Business.Risks.Mappings
{
    public class RiskTrackingReportMappings : Profile
    {
        public RiskTrackingReportMappings()
        {
            CreateMap<CreateRiskTrackingReportCommand, RiskTrackingReport>();
            {
                CreateMap<RiskActionProposalDto, RiskActionProposal>();
            }
        }
    }
}
