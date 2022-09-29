using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Procedures.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.Procedures.Commands.Update;
using DigitNow.Domain.DocumentManagement.Public.Procedures.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Procedures.Mappings
{
    public class ProcedureMapings : Profile
    {
        public ProcedureMapings()
        {
            CreateMap<CreateProcedureRequest, CreateProcedureCommand>();
            CreateMap<UpdateProcedureRequest, UpdateProcedureCommand>();
        }
    }
}
