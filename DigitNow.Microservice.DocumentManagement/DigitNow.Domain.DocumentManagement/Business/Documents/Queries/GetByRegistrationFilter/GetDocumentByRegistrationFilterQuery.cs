using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetByRegistrationFilter
{
    public class GetDocumentByRegistrationFilterQuery : IQuery<GetDocumentByRegistrationFilterResponse>
    {
        public int RegistrationNumber { get; set; }
        public int RegistrationYear { get; set; }
    }
}
