
namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries
{
    public class GetDocsByRegistrationNumberResponse
    {
        public int RegistrationNumber { get; set; }
        public int Id { get; set; }
        public int DocumentTypeId { get; set; }
    }
}
