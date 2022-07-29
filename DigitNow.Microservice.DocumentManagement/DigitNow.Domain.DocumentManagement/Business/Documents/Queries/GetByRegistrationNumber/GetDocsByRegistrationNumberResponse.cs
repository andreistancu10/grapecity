
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetByRegistrationNumber
{
    public class GetDocsByRegistrationNumberResponse
    {
        public int Id { get; set; }

        public int RegistrationNumber { get; set; }

        public DocumentType DocumentType { get; set; }
    }
}
