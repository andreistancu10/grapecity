using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries;

public class GetSpecialRegistersHandler : IQueryHandler<SpecialRegisterQuery, List<SpecialRegisterResponse>>
{
    private readonly IMapper _mapper;
    private readonly ISpecialRegisterService _specialRegisterService;

    public GetSpecialRegistersHandler(DocumentManagementDbContext dbContext,
        IMapper mapper,
        ISpecialRegisterService specialRegisterService)
    {
        _mapper = mapper;
        _specialRegisterService = specialRegisterService;
    }

    public async Task<List<SpecialRegisterResponse>> Handle(SpecialRegisterQuery request, CancellationToken cancellationToken)
    {
        var registers = await _specialRegisterService.FindAllAsync(cancellationToken);

        return _mapper.Map<List<SpecialRegisterResponse>>(registers);
    }
}