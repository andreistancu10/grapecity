using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries;
using DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisters;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Mappings;

public class SpecialRegisterMappings : Profile
{
    public SpecialRegisterMappings()
    {
        CreateMap<SpecialRegister, SpecialRegisterResponse>();
        CreateMap<CreateSpecialRegisterCommand, SpecialRegister>();
        CreateMap<UpdateSpecialRegisterCommand, SpecialRegister>();
    }
}
