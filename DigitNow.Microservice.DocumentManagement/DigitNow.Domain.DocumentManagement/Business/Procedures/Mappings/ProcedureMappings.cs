using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Procedures.Commands.Create;
using DigitNow.Domain.DocumentManagement.Data.Entities.Procedures;

namespace DigitNow.Domain.DocumentManagement.Business.Procedures.Mappings
{
    internal class ProcedureMappings : Profile
    {
        public ProcedureMappings()
        {
            CreateMap<CreateProcedureCommand, Procedure>();

        }
    }
}
