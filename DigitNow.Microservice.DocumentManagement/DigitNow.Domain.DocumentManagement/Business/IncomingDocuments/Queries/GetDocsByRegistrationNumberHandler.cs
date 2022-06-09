using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.Queries;
using HTSS.Platform.Core.CQRS;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;

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
            IList<IncomingDocument> result = await _queryService.GetDocsByRegistrationNumber(request.RegistrationNumber, cancellationToken);
            return _mapper.Map<List<GetDocsByRegistrationNumberResponse>>(result);
        }
    }
}
