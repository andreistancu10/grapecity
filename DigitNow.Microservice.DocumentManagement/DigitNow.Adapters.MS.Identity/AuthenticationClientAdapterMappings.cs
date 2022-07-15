using AutoMapper;
using DigitNow.Adapters.MS.Identity.Poco;
using Domain.Authentication.Contracts;

namespace DigitNow.Adapters.MS.Identity
{
    public class AuthenticationClientAdapterMappings : Profile
    {
        public AuthenticationClientAdapterMappings()
        {
            CreateMap<IGetUserExtension, User>();
        }
    }
}
