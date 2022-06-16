
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries
{
    public class GetDocsByRegistrationNumberQuery : IQuery<GetDocsByRegistrationNumberResponse>
    {
        public int RegistrationNumber { get; set; }
        public int Year { get; set; }
    }
}
