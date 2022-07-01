using AutoMapper;
using DigitNow.Adapters.MS.Catalog;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using HTSS.Platform.Core.CQRS;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries.GetRegistrationProof
{
    public class GetRegistrationProofHandler : IQueryHandler<GetRegistrationProofQuery, GetRegistrationProofResponse>
    {
        private readonly IMapper _mapper;
        private readonly IIncomingDocumentService _incomingDocumentService;
        private readonly ICatalogAdapterClient _catalogAdapterClient;

        public GetRegistrationProofHandler(IMapper mapper, IIncomingDocumentService incomingDocumentService, ICatalogAdapterClient catalogAdapterClient)
        {
            _mapper = mapper;
            _incomingDocumentService = incomingDocumentService;
            _catalogAdapterClient = catalogAdapterClient;
           
        }
        public async Task<GetRegistrationProofResponse> Handle(GetRegistrationProofQuery request, CancellationToken cancellationToken)
        {
            var result = await _incomingDocumentService.GetIncomingDocumentById(request.Id,  cancellationToken);
            if(result == null)
            {
                return null;
            }

            var documentType = await _catalogAdapterClient.GetDocumentTypeById(result.DocumentTypeId, cancellationToken);
            var registrationProof = _mapper.Map<GetRegistrationProofResponse>(result);

            registrationProof.DocumentType = documentType.Name;
            return registrationProof;
        }
    }
}
