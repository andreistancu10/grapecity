using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries.GetSpecialRegisters
{
    public class GetSpecialRegistersQuery : AbstractFilterModel<SpecialRegister>, IQuery<ResultPagedList<SpecialRegisterResponse>>
    {
        public long? Id { get; set; }
        public string Name { get; set; }
    }
}