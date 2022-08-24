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
            CreateMap<SpecificObjective, GetSpecificObjectiveByIdResponse>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.ObjectiveId))
                .ForMember(c => c.Code, opt => opt.MapFrom(src => src.Objective.Code))
                .ForMember(c => c.State, opt => opt.MapFrom(src => src.Objective.State))
                .ForMember(c => c.Title, opt => opt.MapFrom(src => src.Objective.Title))
                .ForMember(c => c.Details, opt => opt.MapFrom(src => src.Objective.Details))
                .ForMember(c => c.ModificationMotive, opt => opt.MapFrom(src => src.Objective.ModificationMotive))
                .ForPath(c => c.AssociatedGeneralObjective.Code, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.Code))
                .ForPath(c => c.AssociatedGeneralObjective.State, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.State))
                .ForPath(c => c.AssociatedGeneralObjective.Title, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.Title))
                .ForPath(c => c.AssociatedGeneralObjective.Details, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.Details))
                .ForPath(c => c.AssociatedGeneralObjective.ModificationMotive, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.ModificationMotive))
                .ForPath(c => c.FunctionaryId, opt => opt.MapFrom(src => src.SpecificObjectiveFunctionarys.Select(x => x.FunctionaryId)))
                .ForPath(c => c.ObjectiveUploadedFiles, opt => opt.MapFrom(src => src.Objective.ObjectiveUploadedFiles));

            CreateMap<SpecificObjective, GetAllByUserDepartmentSpecificObjectiveResponse>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.ObjectiveId))
                .ForMember(c => c.Code, opt => opt.MapFrom(src => src.Objective.Code))
                .ForMember(c => c.Title, opt => opt.MapFrom(src => src.Objective.Title));
        }
    }
}
