using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class ObjectiveViewModelMapping : Profile
    {
        public ObjectiveViewModelMapping()
        {
            CreateMap<VirtualObjectiveAggregate, SpecificObjectiveViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.VirtualObjective.Id))
                .ForMember(dest => dest.AssociatedGeneralObjective, opt => opt.MapFrom(src => src.VirtualObjective.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.VirtualObjective))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.VirtualObjective.CreatedAt))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.VirtualObjective.CreatedBy))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.VirtualObjective.Id))
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.VirtualObjective.Id))
                .ForMember(dest => dest.FunctionaryId, opt => opt.MapFrom(src => src.VirtualObjective.Id))
                .ForMember(dest => dest.GeneralObjectiveId, opt => opt.MapFrom(src => src.VirtualObjective.Id))
                .ForMember(dest => dest.ModificationMotive, opt => opt.MapFrom(src => src.VirtualObjective.Id))
                .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.VirtualObjective.ModifiedAt))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.VirtualObjective.ModifiedBy))
                .ForMember(dest => dest.ObjectiveUploadedFiles, opt => opt.MapFrom(src => src.VirtualObjective.Id))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.VirtualObjective.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.VirtualObjective.Id));
        }
    }
}