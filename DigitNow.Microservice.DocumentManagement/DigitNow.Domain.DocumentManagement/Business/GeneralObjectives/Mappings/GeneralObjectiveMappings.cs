using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Queries.GetAll;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Mappings
{
    public class GeneralObjectiveMappings : Profile
    {
        public GeneralObjectiveMappings()
        {
           CreateMap<GeneralObjective, GetAllGeneralActiveObjectiveResponse>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.ObjectiveId))
                .ForMember(c => c.Code, opt => opt.MapFrom(src => src.Objective.Code))
                .ForMember(c => c.Title, opt => opt.MapFrom(src => src.Objective.Title));
        }
    }
}
