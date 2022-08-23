using AutoMapper;
using DigitNow.Adapters.MS.Catalog;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Export.Pdf.Generators;
using DigitNow.Domain.DocumentManagement.Contracts.Documents;
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
        private readonly IPdfDocumentGenerator _pdfDocumentGenerator;


        public GetRegistrationProofHandler(
            IMapper mapper, 
            IIncomingDocumentService incomingDocumentService, 
            ICatalogAdapterClient catalogAdapterClient,
            IPdfDocumentGenerator pdfDocumentGenerator
            )
        {
            _mapper = mapper;
            _incomingDocumentService = incomingDocumentService;
            _catalogAdapterClient = catalogAdapterClient;
            _pdfDocumentGenerator = pdfDocumentGenerator;
        }
        public async Task<GetRegistrationProofResponse> Handle(GetRegistrationProofQuery request, CancellationToken cancellationToken)
        {

            var result = await _incomingDocumentService.FindFirstAsync(request.Id, cancellationToken);
            if (result == null)
            {
                return null;
            }

            var documentType = await _catalogAdapterClient.GetDocumentTypeByIdAsync(result.DocumentTypeId, cancellationToken);
            var fileContent = _pdfDocumentGenerator.GeneratePdf((new DocumentPdfDetails
            {
                IssuerName = result.IssuerName,
                RegistrationDate = result.Document.RegistrationDate,
                RegistrationNumber = result.Document.RegistrationNumber,
                ResolutionPeriod = result.ResolutionPeriod,
                DocumentType = documentType.Name,
                CityHall = "Primaria Bucuresti",
                InstitutionHeader = "Primaria Bucuresti"
            }), "dovada_inregistrare_document_intrare.pdf");


            return _mapper.Map<FileContent, GetRegistrationProofResponse>(fileContent);
        }
    }
}
