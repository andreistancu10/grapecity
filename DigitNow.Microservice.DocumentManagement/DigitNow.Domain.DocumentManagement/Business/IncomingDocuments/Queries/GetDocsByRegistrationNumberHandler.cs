using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.Queries;
using HTSS.Platform.Core.CQRS;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries
{
    public class GetDocsByRegistrationNumberHandler : IQueryHandler<GetDocsByRegistrationNumberQuery, List<GetDocsByRegistrationNumberResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IDocumentsQueryService _queryService;

        public GetDocsByRegistrationNumberHandler(IMapper mapper, IDocumentsQueryService queryService)
        {
            _mapper = mapper;
            _queryService = queryService;
        }
        public async Task<List<GetDocsByRegistrationNumberResponse>> Handle(GetDocsByRegistrationNumberQuery request, CancellationToken cancellationToken)
        {
            request.Year = request.Year == 0 ? DateTime.Now.Year : request.Year;

            var result = await _queryService.GetDocsByRegistrationNumberAndYear(request.RegistrationNumber, request.Year, cancellationToken);
            return _mapper.Map<List<GetDocsByRegistrationNumberResponse>>(result);
        }
    }
}
