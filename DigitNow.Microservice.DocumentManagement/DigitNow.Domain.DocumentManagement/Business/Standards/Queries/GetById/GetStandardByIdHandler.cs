using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Standards.Queries.GetById
{
    public class GetStandardByIdHandler : IQueryHandler<GetStandardByIdQuery, GetStandardByIdResponse>
    {
        private readonly IMapper _mapper;
        private readonly IStandardService _standardService;
        public GetStandardByIdHandler(
            IMapper mapper,
            IStandardService standardService)
        {
            _mapper = mapper;
            _standardService = standardService;
        }

        public async Task<GetStandardByIdResponse> Handle(GetStandardByIdQuery request, CancellationToken cancellationToken)
        {
            var standard = await _standardService.FindQuery()
                .AsNoTracking()
                .Include(x => x.StandardFunctionaries)
                .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

            if (standard == null)
            {
                return null;
            }

            return _mapper.Map<GetStandardByIdResponse>(standard);
        }
    }
}
