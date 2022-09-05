using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
using DigitNow.Domain.DocumentManagement.Data.Entities.Activities;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class GetActivityViewModelMapping : Profile
    {
        public GetActivityViewModelMapping()
        {
            CreateMap<Activity, GetActivityViewModel>()
                .ForPath(c => c.AssociatedGeneralObjective, opt => opt.MapFrom(src => src.AssociatedGeneralObjective))
                .ForPath(c => c.AssociatedGeneralObjective.Code, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.Code))
                .ForPath(c => c.AssociatedGeneralObjective.State, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.State))
                .ForPath(c => c.AssociatedGeneralObjective.Title, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.Title))
                .ForPath(c => c.AssociatedGeneralObjective.Details, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.Details))
                .ForPath(c => c.AssociatedGeneralObjective.ModificationMotive, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.ModificationMotive))
                .ForPath(c => c.AssociatedSpecificObjective, opt => opt.MapFrom(src => src.AssociatedSpecificObjective))
                .ForPath(c => c.AssociatedSpecificObjective.Code, opt => opt.MapFrom(src => src.AssociatedSpecificObjective.Objective.Code))
                .ForPath(c => c.AssociatedSpecificObjective.State, opt => opt.MapFrom(src => src.AssociatedSpecificObjective.Objective.State))
                .ForPath(c => c.AssociatedSpecificObjective.Title, opt => opt.MapFrom(src => src.AssociatedSpecificObjective.Objective.Title))
                .ForPath(c => c.AssociatedSpecificObjective.Details, opt => opt.MapFrom(src => src.AssociatedSpecificObjective.Objective.Details))
                .ForPath(c => c.AssociatedSpecificObjective.ModificationMotive, opt => opt.MapFrom(src => src.AssociatedGeneralObjective.Objective.ModificationMotive))
            .ForPath(c => c.FunctionaryIds, opt => opt.MapFrom(src => src.ActivityFunctionarys.Select(x => x.FunctionaryId).ToList()));

            CreateMap<GeneralObjective, GeneralObjectiveDto>()
                .ForMember(c => c.ObjectiveId, opt => opt.MapFrom(src => src.ObjectiveId))
                .ForMember(c => c.Code, opt => opt.MapFrom(src => src.Objective.Code))
                .ForMember(c => c.State, opt => opt.MapFrom(src => src.Objective.State))
                .ForMember(c => c.Title, opt => opt.MapFrom(src => src.Objective.Title))
                .ForMember(c => c.Details, opt => opt.MapFrom(src => src.Objective.Details))
                .ForMember(c => c.ModificationMotive, opt => opt.MapFrom(src => src.Objective.ModificationMotive));

            CreateMap<SpecificObjective, SpecificObjectiveDto>()
                .ForMember(c => c.ObjectiveId, opt => opt.MapFrom(src => src.ObjectiveId))
                .ForMember(c => c.Code, opt => opt.MapFrom(src => src.Objective.Code))
                .ForMember(c => c.State, opt => opt.MapFrom(src => src.Objective.State))
                .ForMember(c => c.Title, opt => opt.MapFrom(src => src.Objective.Title))
                .ForMember(c => c.Details, opt => opt.MapFrom(src => src.Objective.Details))
                .ForMember(c => c.ModificationMotive, opt => opt.MapFrom(src => src.Objective.ModificationMotive));
        }
    }
}
