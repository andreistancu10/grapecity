using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.ContactDetails.Queries;

public class GetContactDetailsByIdentificationNumberMapping : Profile
{
    public GetContactDetailsByIdentificationNumberMapping()
    {
        CreateMap<ContactDetail, GetContactDetailsByIdentificationNumberResponse>();
    }
}