using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Forms.Dtos;
using DigitNow.Domain.DocumentManagement.Business.Forms.Queries;
using DigitNow.Domain.DocumentManagement.Business.Forms.Queries.Filter;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Forms.Mapping
{
    public class FormMappings : Profile
    {
        public FormMappings()
        {
            CreateMap<Form, FormDto>();
            CreateMap<Form, FilterFormsResponse>();
            CreateMap<FormFieldMapping, FormFieldMappingDto>()
                .ForMember(c => c.Field.Name, opt => opt.MapFrom(src => src.FormField.Name))
                .ForMember(c => c.Field.Context, opt => opt.MapFrom(src => src.FormField.Context))
                .ForMember(c => c.Field.FieldType, opt => opt.MapFrom(src => src.FormField.FieldType));
        }
    }
}