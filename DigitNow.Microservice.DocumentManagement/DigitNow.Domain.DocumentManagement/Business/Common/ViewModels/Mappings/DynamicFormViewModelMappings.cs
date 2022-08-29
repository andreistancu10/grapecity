using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class DynamicFormViewModelMappings : Profile
    {
        public DynamicFormViewModelMappings()
        {
            CreateMap<DynamicForm, DynamicFormViewModel>()
                .ForMember(dest => dest.DynamicFormDetails.Context, opt => opt.MapFrom(src => src.Context))
                .ForMember(dest => dest.DynamicFormDetails.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.DynamicFormDetails.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DynamicFormDetails.Label, opt => opt.MapFrom(src => src.Label))
                .ForMember(dest => dest.DynamicFormDetails.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DynamicFormControls, opt => opt.MapFrom(src => src.Name));
        }
    }
}