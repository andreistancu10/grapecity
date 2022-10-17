using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Standards.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.Standards.Queries.GetById;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Standards.Mappings
{
    public class StandardMappings : Profile
    {
        public StandardMappings()
        {
            CreateMap<CreateStandardCommand, Standard>();
            CreateMap<Standard, GetStandardByIdResponse>();
            CreateMap<StandardFunctionary, StandardFunctionaryResponse>();
        }
    }
}
