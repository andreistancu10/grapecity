using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class DynamicFormViewModelMappings : Profile
    {
        public DynamicFormViewModelMappings()
        {
            CreateMap<DynamicForm, DynamicFormViewModel>()
                .ForPath(dest => dest.DynamicFormDetails.Context, opt => opt.MapFrom(src => src.Context))
                .ForPath(dest => dest.DynamicFormDetails.Description, opt => opt.MapFrom(src => src.Description))
                .ForPath(dest => dest.DynamicFormDetails.Id, opt => opt.MapFrom(src => src.Id))
                .ForPath(dest => dest.DynamicFormDetails.Label, opt => opt.MapFrom(src => src.Label))
                .ForPath(dest => dest.DynamicFormDetails.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}