using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Risks.Commands.CreateRisk;
using DigitNow.Domain.DocumentManagement.Business.Risks.Commands.CreateRiskTrackingReport;
using DigitNow.Domain.DocumentManagement.Business.Risks.Commands.Update;
using DigitNow.Domain.DocumentManagement.Public.Risks.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Risks.Mappings
{
    public class RiskMappings : Profile
    {
        public RiskMappings()
        {
            CreateMap<CreateRiskRequest, CreateRiskCommand>();
            CreateMap<UpdateRiskRequest, UpdateRiskCommand>();

            CreateMap<CreateRiskTrackingReportRequest, CreateRiskTrackingReportCommand>();
        }
    }
}
