using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments;
using DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments.Queries;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries
{
    public class GetDocsByIdHandler : IQueryHandler<GetDocsByIdQuery, List<GetDocsByIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IOutgoingDocumentsQueryService _queryService;

        public GetDocsByIdHandler(IMapper mapper, IOutgoingDocumentsQueryService queryService)
        {
            _mapper = mapper;
            _queryService = queryService;
        }
        public async Task<List<GetDocsByIdResponse>> Handle(GetDocsByIdQuery request, CancellationToken cancellationToken)
        {
            IList<OutgoingDocument> result = await _queryService.GetDocsById(request.Id, cancellationToken);
            return _mapper.Map<List<GetDocsByIdResponse>>(result);
        }
    }
}
