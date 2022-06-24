using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetByRegistrationNumber
{
    public class GetDocsByRegistrationNumberQuery : IQuery<GetDocsByRegistrationNumberResponse>
    {
        public int RegistrationNumber { get; set; }
        public int Year { get; set; }
        public int DocumentType { get; set; }
    }
}
