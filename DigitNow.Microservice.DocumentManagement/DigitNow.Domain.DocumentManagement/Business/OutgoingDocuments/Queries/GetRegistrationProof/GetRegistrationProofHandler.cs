using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.Queries;
using HTSS.Platform.Core.CQRS;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries.GetRegistrationProof
{
    public class GetRegistrationProofHandler : IQueryHandler<GetRegistrationProofQuery, GetRegistrationProofResponse>
    {
        private readonly IMapper _mapper;
        private readonly IDocumentsQueryService _queryService;

        public GetRegistrationProofHandler(IMapper mapper, IDocumentsQueryService queryService)
        {
            _mapper = mapper;
            _queryService = queryService;
        }
        public async Task<GetRegistrationProofResponse> Handle(GetRegistrationProofQuery request, CancellationToken cancellationToken)
        {
            var result = await _queryService.GetOutgoingDocumentById(request.Id,  cancellationToken);
            return _mapper.Map<GetRegistrationProofResponse>(result);
        }
    }
}
