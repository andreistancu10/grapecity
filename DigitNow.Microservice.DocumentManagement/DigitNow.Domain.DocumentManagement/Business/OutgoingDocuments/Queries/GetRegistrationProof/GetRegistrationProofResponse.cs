using System;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries.GetRegistrationProof
{
    public class GetRegistrationProofResponse
    {
        public int RegistrationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string RecipientName { get; set; }
        public string DocumentType { get; set; }
    }
}
