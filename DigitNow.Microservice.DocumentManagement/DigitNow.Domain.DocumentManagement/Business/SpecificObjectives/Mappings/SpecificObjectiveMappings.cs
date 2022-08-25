using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Queries.GetAll;
using DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Mappings
{
    public class SpecificObjectiveMappings : Profile
    {
        public SpecificObjectiveMappings()
        {
           CreateMap<SpecificObjective, GetAllByUserDepartmentSpecificObjectiveResponse>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.ObjectiveId))
                .ForMember(c => c.Code, opt => opt.MapFrom(src => src.Objective.Code))
                .ForMember(c => c.Title, opt => opt.MapFrom(src => src.Objective.Title));
        }
    }
}
