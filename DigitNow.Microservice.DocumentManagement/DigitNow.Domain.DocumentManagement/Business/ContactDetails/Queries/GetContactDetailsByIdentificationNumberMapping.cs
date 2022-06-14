using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.ContactDetails;

namespace DigitNow.Domain.DocumentManagement.Business.ContactDetails.Queries
{
    public class GetContactDetailsByIdentificationNumberMapping : Profile
    {
        public GetContactDetailsByIdentificationNumberMapping()
        {
            CreateMap<ContactDetail, GetContactDetailsByIdentificationNumberResponse>();
        }
    }
}
