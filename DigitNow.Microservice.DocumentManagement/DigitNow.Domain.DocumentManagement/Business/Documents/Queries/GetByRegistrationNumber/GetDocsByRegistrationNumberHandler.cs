using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetByRegistrationNumber
{
    public class GetDocsByRegistrationNumberHandler : IQueryHandler<GetDocsByRegistrationNumberQuery, GetDocsByRegistrationNumberResponse>
    {
        private readonly IMapper _mapper;
        private readonly IDocumentService _documentService;

        public GetDocsByRegistrationNumberHandler(IMapper mapper, IDocumentService documentService)
        {
            _mapper = mapper;
            _documentService = documentService;
        }
        public async Task<GetDocsByRegistrationNumberResponse> Handle(GetDocsByRegistrationNumberQuery request, CancellationToken cancellationToken)
        {
            var targetYear = request.Year <= 0 ? DateTime.Now.Year : request.Year;

            var documents = await _documentService.FindByRegistrationAsync(request.RegistrationNumber, targetYear, cancellationToken);

            return _mapper.Map<GetDocsByRegistrationNumberResponse>(documents);
        }
    }
}
