using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Commands.Update;
using DigitNow.Domain.DocumentManagement.Public.SpecialRegisters.Models;

namespace DigitNow.Domain.DocumentManagement.Public.SpecialRegisters.Mappings;

public class SpecialRegisterMapping : Profile
{
    public SpecialRegisterMapping()
    {
        CreateMap<CreateSpecialRegisterRequest, CreateSpecialRegisterCommand>();
        CreateMap<UpdateSpecialRegisterRequest, UpdateSpecialRegisterCommand>();
    }
}