using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class GetObjectiveViewModelMapping : Profile
    {
        public GetObjectiveViewModelMapping()
        {
            CreateMap<SpecificObjectiveAggregate, GetSpecificObjectiveViewModel>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.SpecificObjective.ObjectiveId))
                .ForMember(c => c.Code, opt => opt.MapFrom(src => src.SpecificObjective.Objective.Code))
                .ForMember(c => c.State, opt => opt.MapFrom(src => src.SpecificObjective.Objective.State))
                .ForMember(c => c.Title, opt => opt.MapFrom(src => src.SpecificObjective.Objective.Title))
                .ForMember(c => c.Details, opt => opt.MapFrom(src => src.SpecificObjective.Objective.Details))
                .ForMember(c => c.ModificationMotive, opt => opt.MapFrom(src => src.SpecificObjective.Objective.ModificationMotive))
                .ForPath(c => c.AssociatedGeneralObjective.Code, opt => opt.MapFrom(src => src.SpecificObjective.AssociatedGeneralObjective.Objective.Code))
                .ForPath(c => c.AssociatedGeneralObjective.State, opt => opt.MapFrom(src => src.SpecificObjective.AssociatedGeneralObjective.Objective.State))
                .ForPath(c => c.AssociatedGeneralObjective.Title, opt => opt.MapFrom(src => src.SpecificObjective.AssociatedGeneralObjective.Objective.Title))
                .ForPath(c => c.AssociatedGeneralObjective.Details, opt => opt.MapFrom(src => src.SpecificObjective.AssociatedGeneralObjective.Objective.Details))
                .ForPath(c => c.AssociatedGeneralObjective.ModificationMotive, opt => opt.MapFrom(src => src.SpecificObjective.AssociatedGeneralObjective.Objective.ModificationMotive))
                .ForPath(c => c.FunctionaryId, opt => opt.MapFrom(src => src.SpecificObjective.SpecificObjectiveFunctionarys.Select(x => x.FunctionaryId)))
                .ForPath(c => c.ObjectiveUploadedFiles, opt => opt.MapFrom(src => src.DocumentFileMappingModels));

            CreateMap<GeneralObjectiveAggregate, GeneralObjectiveViewModel>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.GeneralObjective.ObjectiveId))
                .ForMember(c => c.Code, opt => opt.MapFrom(src => src.GeneralObjective.Objective.Code))
                .ForMember(c => c.State, opt => opt.MapFrom(src => src.GeneralObjective.Objective.State))
                .ForMember(c => c.Title, opt => opt.MapFrom(src => src.GeneralObjective.Objective.Title))
                .ForMember(c => c.Details, opt => opt.MapFrom(src => src.GeneralObjective.Objective.Details))
                .ForMember(c => c.ModificationMotive, opt => opt.MapFrom(src => src.GeneralObjective.Objective.ModificationMotive))
                .ForMember(c => c.ObjectiveUploadedFiles, opt => opt.MapFrom(src => src.DocumentFileMappingModels));
        }
    }
}