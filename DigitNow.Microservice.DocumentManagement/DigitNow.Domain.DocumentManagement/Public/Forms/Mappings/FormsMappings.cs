using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Forms.Commands;
using DigitNow.Domain.DocumentManagement.Business.Forms.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Public.Forms.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Forms.Mappings
{
    public class FormsMappings : Profile
    {
        public FormsMappings()
        {
            CreateMap<GetFormByIdRequest, GetFormByIdQuery>();
            CreateMap<SaveFormDataRequest, SaveFormDataCommand>();
        }
    }
}