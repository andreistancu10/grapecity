using AutoMapper;
using DigitNow.Adapters.MS.Catalog;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Export.Pdf.Generators;
using DigitNow.Domain.DocumentManagement.Contracts.Documents;
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
        private readonly IPdfDocumentGenerator _pdfDocumentGenerator;

        public GetRegistrationProofHandler(
            IOutgoingDocumentService outgoingDocumentService,
            ICatalogAdapterClient catalogAdapterClient,
            IPdfDocumentGenerator pdfDocumentGenerator, 
            IMapper mapper)
        {
            _outgoingDocumentService = outgoingDocumentService;
            _catalogAdapterClient = catalogAdapterClient;
            _pdfDocumentGenerator = pdfDocumentGenerator;
            _mapper = mapper;

        }
        public async Task<GetRegistrationProofResponse> Handle(GetRegistrationProofQuery request, CancellationToken cancellationToken)
        {
            var result = await _outgoingDocumentService.FindFirstAsync(request.Id, cancellationToken);
            if(result == null)
            {
                return null;
            }

            var documentType = await _catalogAdapterClient.GetDocumentTypeByIdAsync(result.DocumentTypeId, cancellationToken);
            var fileContent = await _pdfDocumentGenerator.GeneratePdf((new DocumentPdfDetails
            {
                IssuerName = result.RecipientName,
                RegistrationDate = result.Document.RegistrationDate,
                RegistrationNumber = result.Document.RegistrationNumber,
                ResolutionPeriod = null,
                DocumentType = documentType.Name,
                CityHall = "Primaria Bucuresti",
                InstitutionHeader = "Primaria Bucuresti"
            }), "Business/Common/Export/Pdf/Templates/Html/registration_proof_template.html", "dovada_inregistrare_document_iesire.pdf");


            return _mapper.Map<FileContent, GetRegistrationProofResponse>(fileContent);
        }
    }
}
