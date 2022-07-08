using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries.GetSpecialRegisters;

public class GetSpecialRegistersHandler : IQueryHandler<SpecialRegisterQuery, ResultPagedList<SpecialRegisterResponse>>
{
    private readonly IMapper _mapper;
    private readonly ISpecialRegisterService _specialRegisterService;

    public GetSpecialRegistersHandler(
        IMapper mapper,
        ISpecialRegisterService specialRegisterService)
    {
        _mapper = mapper;
        _specialRegisterService = specialRegisterService;
    }

    public async Task<ResultPagedList<SpecialRegisterResponse>> Handle(SpecialRegisterQuery request, CancellationToken cancellationToken)
    {
        var registers = await _specialRegisterService.FindAllAsync(cancellationToken);

        var pagingHeader = new PagingHeader(registers.Count, 1, registers.Count, 1);
        var result = new ResultPagedList<SpecialRegisterResponse>(pagingHeader, _mapper.Map<List<SpecialRegisterResponse>>(registers));

        return result;
    }
}