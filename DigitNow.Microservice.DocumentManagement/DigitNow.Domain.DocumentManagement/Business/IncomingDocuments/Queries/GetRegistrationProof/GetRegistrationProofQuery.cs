using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries.GetRegistrationProof
{
    public class GetRegistrationProofQuery: IQuery<GetRegistrationProofResponse>
    {
        public int Id { get; set; }
    }
}
