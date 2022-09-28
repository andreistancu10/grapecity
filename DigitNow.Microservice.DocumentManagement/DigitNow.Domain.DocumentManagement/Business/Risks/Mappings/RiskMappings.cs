using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Risks.Commands.CreateRisk;
using DigitNow.Domain.DocumentManagement.Business.Risks.Commands.Update;
using DigitNow.Domain.DocumentManagement.Data.Entities.Risks;

namespace DigitNow.Domain.DocumentManagement.Business.Risks.Mappings
{
    public class RiskMappings : Profile
    {
        public RiskMappings()
        {
            CreateMap<CreateRiskCommand, Risk>();
            CreateMap<UpdateRiskCommand, Risk>();

        }
    }
}
