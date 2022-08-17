using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Forms.Dtos;
using DigitNow.Domain.DocumentManagement.Business.Forms.Queries.Filter;
using DigitNow.Domain.DocumentManagement.Business.Forms.Queries.GetFormById;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Forms.Mapping
{
    public class FormMappings : Profile
    {
        public FormMappings()
        {
            CreateMap<Form, FormDto>();
            CreateMap<Form, FilterFormsResponse>();
            CreateMap<Form, GetFormByIdResponse>();
        }
    }
}