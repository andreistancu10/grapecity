using DigitNow.Domain.DocumentManagement.Business.Common.Export.Pdf.Poco;
using DigitNow.Domain.DocumentManagement.Contracts.Documents;
using System;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Export.Pdf.Generators
{
    public interface IPdfDocumentGenerator
    {
        FileContent GeneratePdf(DocumentPdfDetails documentPdfDetails, string fileName);
    }
    public class PdfDocumentGenerator: IPdfDocumentGenerator
    {
        private readonly IPdfGenerator _pdfGenerator;
        public PdfDocumentGenerator(IPdfGenerator pdfGenerator)
        {
            _pdfGenerator = pdfGenerator;
        }

        public FileContent GeneratePdf(DocumentPdfDetails documentPdfDetails, string fileName)
        {
            return _pdfGenerator
             .SetTemplateName("registration_proof_template.html")
             .SetToken(new PdfToken("{{institutionHeader}}", documentPdfDetails.InstitutionHeader != null ? documentPdfDetails.InstitutionHeader : "" ))
             .SetToken(new PdfToken("{{senderName}}", documentPdfDetails.IssuerName))
             .SetToken(new PdfToken("{{cityHall}}", documentPdfDetails.CityHall))
             .SetToken(new PdfToken("{{docType}}", documentPdfDetails.DocumentType))
             .SetToken(new PdfToken("{{registrationNumber}}", documentPdfDetails.RegistrationNumber.ToString()))
             .SetToken(new PdfToken("{{registrationDate}}", documentPdfDetails.RegistrationDate.ToShortDateString()))
             .SetToken(new PdfToken("{{resolutionPeriodText}}", documentPdfDetails.ResolutionPeriod.HasValue ?
                    $"Termenul maxim de solutionare este {documentPdfDetails.ResolutionPeriod} zile" : ""))
             .SetToken(new PdfToken("{{currentDate}}", DateTime.UtcNow.ToShortDateString()))
             .Generate(fileName);
        }
    }
}
