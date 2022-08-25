using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class FormControlViewModelMappings : Profile
    {
        public FormControlViewModelMappings()
        {
            CreateMap<FormControlAggregate, FormControlViewModel>()
                .ForMember(c => c.FormId, opt => opt.MapFrom(src => src.FormFieldMapping.FormId))
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.FormFieldMapping.Id))
                .ForMember(c => c.Key, opt => opt.MapFrom(src => src.FormFieldMapping.Key))
                .ForMember(c => c.Label, opt => opt.MapFrom(src => src.FormFieldMapping.Label))
                .ForMember(c => c.InitialValue, opt => opt.MapFrom(src => src.FormFieldMapping.InitialValue))
                .ForMember(c => c.OrderNumber, opt => opt.MapFrom(src => src.FormFieldMapping.Order))
                .ForMember(c => c.Required, opt => opt.MapFrom(src => src.FormFieldMapping.Required))
                .ForMember(c => c.FieldType, opt => opt.MapFrom<FieldTypeResolver>());
        }

        private class FieldTypeResolver : IValueResolver<FormControlAggregate, FormControlViewModel, int>
        {
            public int Resolve(FormControlAggregate source, FormControlViewModel destination, int destMember, ResolutionContext context)
            {
                return (int)source.FormFields.First(c => c.Id == source.FormFieldMapping.FormFieldId).DynamicFieldType;
            }
        }
    }
}