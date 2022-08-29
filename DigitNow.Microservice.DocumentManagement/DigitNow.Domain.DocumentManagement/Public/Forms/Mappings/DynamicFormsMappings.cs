using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.DynamicForms.Commands;
using DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.Filter;
using DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.GetDynamicFormById;
using DigitNow.Domain.DocumentManagement.Public.Forms.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Forms.Mappings
{
    public class DynamicFormsMappings : Profile
    {
        public DynamicFormsMappings()
        {
            CreateMap<FilterDynamicFormsRequest, DynamicFilterFormsQuery>();
            CreateMap<GetFormByIdRequest, GetDynamicFormByIdQuery>();
            CreateMap<SaveDynamicFormDataRequest, SaveDynamicFormDataCommand>();
        }
    }
}