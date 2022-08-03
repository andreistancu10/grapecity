using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisters;
using DigitNow.Domain.DocumentManagement.utils;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries.GetSpecialRegisters
{
    public class GetSpecialRegistersHandler : IQueryHandler<GetSpecialRegistersQuery, ResultPagedList<SpecialRegisterResponse>>
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

        public async Task<ResultPagedList<SpecialRegisterResponse>> Handle(GetSpecialRegistersQuery request, CancellationToken cancellationToken)
        {
            var registers = await _specialRegisterService.FindAllAsync(request, cancellationToken);
            var result = new ResultPagedList<SpecialRegisterResponse>(registers.GetHeader(), _mapper.Map<List<SpecialRegisterResponse>>(registers.List));

            return result;
        }
    }
}