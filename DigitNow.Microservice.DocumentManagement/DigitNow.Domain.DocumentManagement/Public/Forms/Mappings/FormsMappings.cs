using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Forms.Commands;
using DigitNow.Domain.DocumentManagement.Business.Forms.Queries.Filter;
using DigitNow.Domain.DocumentManagement.Business.Forms.Queries.GetFormById;
using DigitNow.Domain.DocumentManagement.Public.Forms.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Forms.Mappings
{
    public class FormsMappings : Profile
    {
        public FormsMappings()
        {
            CreateMap<FilterFormsRequest, FilterFormsQuery>();
            CreateMap<GetFormByIdRequest, GetFormByIdQuery>();
            CreateMap<SaveFormDataRequest, SaveFormDataCommand>();
        }
    }
}