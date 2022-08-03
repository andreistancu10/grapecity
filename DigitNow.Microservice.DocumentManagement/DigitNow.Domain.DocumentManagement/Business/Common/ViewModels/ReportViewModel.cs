using System;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class ReportViewModel
    {
        public long Id { get; set; }
        public int RegistrationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public BasicViewModel SpecialRegister { get; set; }
        public int DocumentType { get; set; }
        public BasicViewModel DocumentCategory { get; set; }
        public BasicViewModel Issuer { get; set; }
        public BasicViewModel Recipient { get; set; }
        public BasicViewModel Functionary { get; set; }
        public DateTime? AllocationDate { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public int? CurrentStatus { get; set; }
        public int Expired { get; set; }
    }
}