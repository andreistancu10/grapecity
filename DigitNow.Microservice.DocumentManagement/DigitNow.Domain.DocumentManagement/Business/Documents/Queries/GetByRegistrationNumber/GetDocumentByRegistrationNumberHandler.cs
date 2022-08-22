using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetByRegistrationNumber
{
    public class GetDocumentByRegistrationNumberHandler : IQueryHandler<GetDocumentByRegistrationNumberQuery, GetDocumentByRegistrationNumberResponse>
    {
        private readonly IMapper _mapper;
        private readonly IDocumentService _documentService;

        public GetDocumentByRegistrationNumberHandler(IMapper mapper, IDocumentService documentService)
        {
            _mapper = mapper;
            _documentService = documentService;
        }
        public async Task<GetDocumentByRegistrationNumberResponse> Handle(GetDocumentByRegistrationNumberQuery request, CancellationToken cancellationToken)
        {
            var targetYear = request.Year <= 0 ? DateTime.Now.Year : request.Year;

            var documentsQuery = await _documentService.FindByRegistrationQueryAsync(request.RegistrationNumber, targetYear, cancellationToken);

            var foundDocument = await documentsQuery.FirstOrDefaultAsync(cancellationToken);
            if (foundDocument == null) return null;

            return _mapper.Map<GetDocumentByRegistrationNumberResponse>(foundDocument);
        }
    }
}
