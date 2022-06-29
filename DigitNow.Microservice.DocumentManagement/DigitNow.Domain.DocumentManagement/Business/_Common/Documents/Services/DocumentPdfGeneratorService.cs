using DigitNow.Domain.DocumentManagement.Contracts.Documents;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business._Common.Documents.Services
{
    public class DocumentPdfGeneratorService : PdfGeneratorService, IDocumentPdfGeneratorService
    {
        public Task<FileContent> GenerateIncomingDocRegistrationProofPdfAsync(DocumentPdfDetails incomingDocumentPdfDetails)
        {

            var replaceableStrings = GetReplaceableStrings(incomingDocumentPdfDetails);

            return GeneratePdfAsync("Business/_Common/Documents/HtmlTemplates/registration_proof_template.html", "dovada_inregistrare.pdf", replaceableStrings);
        }

        public Task<FileContent> GenerateOutgoingDocRegistrationProofPdfAsync(DocumentPdfDetails outgoingDocumentPdfDetails)
        {
            var replaceableStrings = GetReplaceableStrings(outgoingDocumentPdfDetails);

            return GeneratePdfAsync("Business/_Common/Documents/HtmlTemplates/registration_proof_template.html", "dovada_inregistrare.pdf", replaceableStrings);
        }

        private List<Tuple<string, string>> GetReplaceableStrings(DocumentPdfDetails documentPdfDetails)
        {
            return new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("{{institutionHeader}}", documentPdfDetails.InstitutionHeader != null ? documentPdfDetails.InstitutionHeader : ""),
                new Tuple<string, string>("{{senderName}}", documentPdfDetails.IssuerName),
                new Tuple<string, string>("{{cityHall}}", documentPdfDetails.CityHall),
                new Tuple<string, string>("{{docType}}", documentPdfDetails.DocumentType),
                new Tuple<string, string>("{{registrationNumber}}", documentPdfDetails.RegistrationNumber.ToString()),
                new Tuple<string, string>("{{registrationDate}}", documentPdfDetails.RegistrationDate.ToShortDateString()),
                new Tuple<string, string>("{{resolutionPeriodText}}", 
                    documentPdfDetails.ResolutionPeriod.HasValue ?
                    $"Termenul maxim de solutionare este {documentPdfDetails.ResolutionPeriod} zile" : ""),
                new Tuple<string, string>("{{currentDate}}", DateTime.UtcNow.ToShortDateString()),
            };
        }
    } 
}
