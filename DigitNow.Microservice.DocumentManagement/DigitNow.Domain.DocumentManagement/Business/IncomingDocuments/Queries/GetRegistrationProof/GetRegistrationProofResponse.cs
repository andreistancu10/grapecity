using System;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries.GetRegistrationProof
{
    public class GetRegistrationProofResponse
    {
        public int RegistrationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string IssuerName { get; set; }
        public string DocumentType { get; set; } 
        public double ResolutionPeriod { get; set; }
    }
}
