using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Standards.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.Standards.Commands.Update;
using DigitNow.Domain.DocumentManagement.Public.Standards.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Standards.Mappings
{
    public class StandardMapping : Profile
    {
        public StandardMapping()
        {
            CreateMap<CreateStandardRequest, CreateStandardCommand>();
            CreateMap<UpdateStandardRequest, UpdateStandardCommand>();
        }
    }
}
