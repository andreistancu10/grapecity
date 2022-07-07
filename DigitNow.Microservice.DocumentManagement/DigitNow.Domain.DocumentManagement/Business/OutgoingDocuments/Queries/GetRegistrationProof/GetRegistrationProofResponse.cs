namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries.GetRegistrationProof
{
    public class GetRegistrationProofResponse
    {
        public string Name { get; set; }

        public string ContentType { get; set; }

        public byte[] Content { get; set; }
    }
}
