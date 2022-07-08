using DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries.GetSpecialRegisters;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries.GetSpecialRegisterById;

public class GetSpecialRegisterByIdQuery : IQuery<SpecialRegisterResponse>
{
    public long Id { get; set; }

    public GetSpecialRegisterByIdQuery(long id)
    {
        Id = id;
    }
}