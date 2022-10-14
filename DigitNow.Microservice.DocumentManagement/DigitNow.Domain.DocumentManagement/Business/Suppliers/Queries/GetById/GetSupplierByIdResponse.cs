using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
using DigitNow.Domain.DocumentManagement.Contracts.Suppliers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Suppliers.Queries.GetById
{
    public class GetSupplierByIdResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public SupplierStatus Status { get; set; }
        public long CertificateRegistration { get; set; }
        public string CommercialRegistration { get; set; }
        public bool VatPayer { get; set; }
        public SupplierCompanyType CompanyType { get; set; }
        public bool RegisteredWorkplace { get; set; }
        public ContactDetailDto RegisteredOfficeContactDetail { get; set; }
        public ContactDetailDto RegisteredWorkplaceContactDetail { get; set; }
        public List<SupplierLegalRepresentativeDto> LegalRepresentatives { get; set; }
    }

   
}
