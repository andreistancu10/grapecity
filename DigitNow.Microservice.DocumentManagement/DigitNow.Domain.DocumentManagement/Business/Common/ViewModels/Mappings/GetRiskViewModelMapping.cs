using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Action = DigitNow.Domain.DocumentManagement.Data.Entities.Action;
using Activity = DigitNow.Domain.DocumentManagement.Data.Entities.Activity;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class GetRiskViewModelMapping : Profile
    {
        public GetRiskViewModelMapping()
        {
            CreateMap<Risk, GetRiskViewModel>();

            CreateMap<RiskControlAction, RiskControlActionDto>();
            CreateMap<Activity, ActivityDto>();
            CreateMap<Action, ActionDto>();
        }
    }
}
