using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
using DigitNow.Domain.DocumentManagement.Public.Common.Models;

namespace DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Mappings;

public class CommonRequestMappings : Profile
{
    public CommonRequestMappings()
    {
        CreateMap<ContactDetailsRequest, ContactDetailDto>();
    }
}