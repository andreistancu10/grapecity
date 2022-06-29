namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries.GetRegistrationProof
{
    public class GetRegistrationProofResponse
    {
        public int RegistrationNumber { get; set; }
        public string RecipientName { get; set; }
        public int DocumentTypeId { get; set; }
    }
}
