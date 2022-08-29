using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.DynamicForms.Dtos;
using DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.Filter;
using DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.GetDynamicFormById;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.DynamicForms.Mapping
{
    public class DynamicFormMappings : Profile
    {
        public DynamicFormMappings()
        {
            CreateMap<DynamicForm, DynamicFormDto>();
            CreateMap<DynamicForm, DynamicFilterFormsResponse>();
            CreateMap<DynamicForm, GetDynamicFormByIdResponse>();
        }
    }
}