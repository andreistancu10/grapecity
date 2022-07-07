using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries.GetRegistrationProof
{
    public class GetRegistrationProofQuery: IQuery<GetRegistrationProofResponse>
    {
        public int Id { get; set; }
    }
}
