using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries.GetSpecialRegisters;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries.GetSpecialRegisterById;

public class GetSpecialRegisterByIdHandler : IQueryHandler<GetSpecialRegisterByIdQuery, SpecialRegisterResponse>
{
    private readonly IMapper _mapper;
    private readonly ISpecialRegisterService _specialRegisterService;

    public GetSpecialRegisterByIdHandler(
        IMapper mapper,
        ISpecialRegisterService specialRegisterService)
    {
        _mapper = mapper;
        _specialRegisterService = specialRegisterService;
    }

    public async Task<SpecialRegisterResponse> Handle(GetSpecialRegisterByIdQuery request, CancellationToken cancellationToken)
    {
        var register = await _specialRegisterService.FindAsync(c => c.Id == request.Id, cancellationToken);
        return _mapper.Map<SpecialRegisterResponse>(register);
    }
}