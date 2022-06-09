using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments;
using DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments.Queries;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries
{
    public class GetDocsByRegistrationNumberHandler : IQueryHandler<GetOutgoingByRegistrationNumberQuery, List<GetDocsByRegistrationNumberResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IOutgoingDocumentsQueryService _queryService;

        public GetDocsByRegistrationNumberHandler(IMapper mapper, IOutgoingDocumentsQueryService queryService)
        {
            _mapper = mapper;
            _queryService = queryService;
        }
        public async Task<List<GetDocsByRegistrationNumberResponse>> Handle(GetOutgoingByRegistrationNumberQuery request, CancellationToken cancellationToken)
        {
            IList<OutgoingDocument> result = await _queryService.GetDocsByRegistrationNumber(request.RegistrationNumber, cancellationToken);
            return _mapper.Map<List<GetDocsByRegistrationNumberResponse>>(result);
        }
    }
}
