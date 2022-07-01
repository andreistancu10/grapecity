using AutoMapper;
using DigitNow.Adapters.MS.Catalog;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using HTSS.Platform.Core.CQRS;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries.GetRegistrationProof
{
    public class GetRegistrationProofHandler : IQueryHandler<GetRegistrationProofQuery, GetRegistrationProofResponse>
    {
        private readonly IMapper _mapper;
        private readonly IOutgoingDocumentService _outgoingDocumentService;
        private readonly ICatalogAdapterClient _catalogAdapterClient;

        public GetRegistrationProofHandler(IMapper mapper, IOutgoingDocumentService outgoingDocumentService, ICatalogAdapterClient catalogAdapterClient) 
        {
            _mapper = mapper;
            _outgoingDocumentService = outgoingDocumentService;
            _catalogAdapterClient = catalogAdapterClient;
        }
        public async Task<GetRegistrationProofResponse> Handle(GetRegistrationProofQuery request, CancellationToken cancellationToken)
        {
            var result = await _outgoingDocumentService.GetDocumentByIdAsync(request.Id, cancellationToken);
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
