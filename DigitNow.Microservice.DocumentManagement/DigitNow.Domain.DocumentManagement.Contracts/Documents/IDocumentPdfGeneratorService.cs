using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Contracts.Documents
{
    public interface IDocumentPdfGeneratorService
    {
        public Task<FileContent> GenerateIncomingDocRegistrationProofPdfAsync(DocumentPdfDetails documentPdfDetails);
        public Task<FileContent> GenerateOutgoingDocRegistrationProofPdfAsync(DocumentPdfDetails documentPdfDetails);

    }
}
