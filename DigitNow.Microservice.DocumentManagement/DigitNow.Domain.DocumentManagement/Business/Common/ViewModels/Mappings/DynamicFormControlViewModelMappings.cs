using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Mappings
{
    public class DynamicFormControlViewModelMappings : Profile
    {
        public DynamicFormControlViewModelMappings()
        {
            CreateMap<DynamicFormControlAggregate, DynamicFormControlViewModel>()
                .ForMember(c => c.DynamicFormId, opt => opt.MapFrom(src => src.DynamicFormFieldMapping.DynamicFormId))
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.DynamicFormFieldMapping.Id))
                .ForMember(c => c.Key, opt => opt.MapFrom(src => src.DynamicFormFieldMapping.Key))
                .ForMember(c => c.Label, opt => opt.MapFrom(src => src.DynamicFormFieldMapping.Label))
                .ForMember(c => c.InitialValue, opt => opt.MapFrom(src => src.DynamicFormFieldMapping.InitialValue))
                .ForMember(c => c.OrderNumber, opt => opt.MapFrom(src => src.DynamicFormFieldMapping.Order))
                .ForMember(c => c.Required, opt => opt.MapFrom(src => src.DynamicFormFieldMapping.Required))
                .ForMember(c => c.FieldType, opt => opt.MapFrom<FieldTypeResolver>());
        }

        private class FieldTypeResolver : IValueResolver<DynamicFormControlAggregate, DynamicFormControlViewModel, int>
        {
            public int Resolve(DynamicFormControlAggregate source, DynamicFormControlViewModel destination, int destMember, ResolutionContext context)
            {
                return (int)source.DynamicFormFields.First(c => c.Id == source.DynamicFormFieldMapping.DynamicFormFieldId).DynamicFieldType;
            }
        }
    }
}