using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetByRegistrationFilter
{
    public class GetDocumentByRegistrationFilterHandler : IQueryHandler<GetDocumentByRegistrationFilterQuery, GetDocumentByRegistrationFilterResponse>
    {
        private readonly IMapper _mapper;
        private readonly IDocumentService _documentService;

        public GetDocumentByRegistrationFilterHandler(IMapper mapper, IDocumentService documentService)
        {
            _mapper = mapper;
            _documentService = documentService;
        }
        public async Task<GetDocumentByRegistrationFilterResponse> Handle(GetDocumentByRegistrationFilterQuery request, CancellationToken cancellationToken)
        {
            var targetYear = request.RegistrationYear <= 0 ? DateTime.Now.Year : request.RegistrationYear;

            var documentsQuery = await _documentService.FindByRegistrationQueryAsync(request.RegistrationNumber, targetYear, cancellationToken);

            var foundDocument = await documentsQuery.FirstOrDefaultAsync(cancellationToken);
            if (foundDocument == null) return null;

            return _mapper.Map<GetDocumentByRegistrationFilterResponse>(foundDocument);
        }
    }
}
