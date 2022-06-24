using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries;

public class GetSpecialRegistersHandler : IQueryHandler<SpecialRegisterQuery, List<SpecialRegisterResponse>>
{
    private readonly DocumentManagementDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetSpecialRegistersHandler(DocumentManagementDbContext dbContext,  IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<SpecialRegisterResponse>> Handle(SpecialRegisterQuery request, CancellationToken cancellationToken)
    {
        var registers = await _dbContext.SpecialRegisters.ToListAsync(cancellationToken);
        ;
        return _mapper.Map<List<SpecialRegisterResponse>>(registers);
    }
}