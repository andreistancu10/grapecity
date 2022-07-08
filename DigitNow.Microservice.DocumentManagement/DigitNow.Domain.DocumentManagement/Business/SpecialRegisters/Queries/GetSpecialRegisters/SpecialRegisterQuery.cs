using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries.GetSpecialRegisters;

public class SpecialRegisterQuery : IQuery<ResultPagedList<SpecialRegisterResponse>>
{
}