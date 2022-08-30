using System;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries.GetDocumentsReports
{
    public class GetDocumentsReportsResponse
    {
        public long Id { get; set; }
        public int RegistrationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public long? SpecialRegisterId { get; set; }
        public int DocumentType { get; set; }
        public int DocumentCategoryId { get; set; }
        public string DocumentCategory { get; set; }
        public string IssuerName { get; set; }
        public int Recipient { get; set; }
        public string Functionary { get; set; }
        public DateTime AllocationDate { get; set; }
        public DateTime ResolutionDate { get; set; }
        public int CurrentStatus { get; set; }
        public int Expires { get; set; }
    }
}