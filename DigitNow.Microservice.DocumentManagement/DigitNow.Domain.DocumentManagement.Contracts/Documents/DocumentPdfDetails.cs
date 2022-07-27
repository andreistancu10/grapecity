using System;

namespace DigitNow.Domain.DocumentManagement.Contracts.Documents
{
    public class DocumentPdfDetails
    {
        public long RegistrationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string IssuerName { get; set; }
        public string DocumentType { get; set; }
        public double? ResolutionPeriod { get; set; }
        public string CityHall { get; set; }
        public string InstitutionHeader { get; set; }
    }
}
