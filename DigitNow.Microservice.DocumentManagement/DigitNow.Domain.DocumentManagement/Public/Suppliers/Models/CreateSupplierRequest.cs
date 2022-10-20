using DigitNow.Domain.DocumentManagement.Contracts.Suppliers.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.Suppliers;
using DigitNow.Domain.DocumentManagement.Public.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.Suppliers.Models
{
    public class CreateSupplierRequest
    {
        public string? Name { get; set; }
        public long? CertificateRegistration { get; set; }
        public string? CommercialRegistration { get; set; }
        public bool? VatPayer { get; set; }
        public SupplierCompanyType? CompanyType { get; set; }
        public bool? RegisteredWorkplace { get; set; }
        public ContactDetailsRequest? RegisteredOfficeContactDetail { get; set; }
        public ContactDetailsRequest? RegisteredWorkplaceContactDetail { get; set; }
        public List<CreateSupplierLegalRepresentativeRequest>? LegalRepresentatives { get; set; }
    }

    public class CreateSupplierLegalRepresentativeRequest
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? RepresentativeQuality { get; set; }
        public string? NationalId { get; set; }
       
    }
}
