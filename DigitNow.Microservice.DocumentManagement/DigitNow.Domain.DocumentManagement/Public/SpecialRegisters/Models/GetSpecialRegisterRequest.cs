using DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisters;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Public.SpecialRegisters.Models
{
    public class GetSpecialRegisterRequest : AbstractFilterModel<SpecialRegister>
    {
        public long? Id { get; set; }
        public string Name { get; set; }
    }
}