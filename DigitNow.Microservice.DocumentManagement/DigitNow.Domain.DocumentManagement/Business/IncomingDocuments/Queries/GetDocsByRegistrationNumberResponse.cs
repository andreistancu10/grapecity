
namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries
{
    public class GetDocsByRegistrationNumberResponse
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public int DocumentTypeId { get; set; }
    }
}
