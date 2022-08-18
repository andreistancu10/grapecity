using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Queries.GetAll;
using DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Mappings
{
    public class GeneralObjectiveMappings : Profile
    {
        public GeneralObjectiveMappings()
        {
            CreateMap<GeneralObjective, GetGeneralObjectiveByIdResponse>()
                .ForMember(c => c.Id , opt => opt.MapFrom(src => src.ObjectiveId))
                .ForMember(c => c.Code, opt => opt.MapFrom(src => src.Objective.Code))
                .ForMember(c => c.State, opt => opt.MapFrom(src => src.Objective.State))
                .ForMember(c => c.Title, opt => opt.MapFrom(src => src.Objective.Title))
                .ForMember(c => c.Details, opt => opt.MapFrom(src => src.Objective.Details))
                .ForMember(c => c.ModificationMotive, opt => opt.MapFrom(src => src.Objective.ModificationMotive))
                .ForPath(c => c.ObjectiveUploadedFiles, opt => opt.MapFrom(src => src.Objective.ObjectiveUploadedFiles));

            CreateMap<GeneralObjective, GetAllGeneralActiveObjectiveResponse>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.ObjectiveId))
                .ForMember(c => c.Code, opt => opt.MapFrom(src => src.Objective.Code))
                .ForMember(c => c.Title, opt => opt.MapFrom(src => src.Objective.Title));
        }
    }
}
