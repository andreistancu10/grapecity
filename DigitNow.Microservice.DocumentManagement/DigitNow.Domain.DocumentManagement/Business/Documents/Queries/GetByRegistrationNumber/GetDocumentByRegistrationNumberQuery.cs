using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetByRegistrationNumber
{
    public class GetDocumentByRegistrationNumberQuery : IQuery<GetDocumentByRegistrationNumberResponse>
    {
        public int RegistrationNumber { get; set; }
        public int Year { get; set; }
    }
}
