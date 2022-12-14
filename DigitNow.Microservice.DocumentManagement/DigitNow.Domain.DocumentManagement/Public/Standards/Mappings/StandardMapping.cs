using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Standards.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.Standards.Commands.Update;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Business.Standards.Queries.GetStandards;
using DigitNow.Domain.DocumentManagement.Public.Standards.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Standards.Mappings
{
    public class StandardMapping : Profile
    {
        public StandardMapping()
        {
            CreateMap<CreateStandardRequest, CreateStandardCommand>();
            CreateMap<UpdateStandardRequest, UpdateStandardCommand>();
            CreateMap<UpdateStandardCommand, Standard>();
        }
    }
}
