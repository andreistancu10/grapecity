using DigitNow.Domain.DocumentManagement.Contracts.Suppliers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Suppliers
{
    public class Supplier : ExtendedEntity
    {
        public SupplierStatus Status { get; set; }
        public string Name { get; set; }
        public long CertificateRegistration { get; set; }
        public string CommercialRegistration { get; set; }
        public bool VatPayer { get; set; }
        public SupplierCompanyType CompanyType { get; set; }
        public bool RegisteredWorkplace { get; set; }


        #region [ References ]

        public ContactDetail RegisteredOfficeContactDetail { get; set; }
        public ContactDetail RegisteredWorkplaceContactDetail { get; set; }
        public virtual List<SupplierLegalRepresentative> LegalRepresentatives { get; set; }

        #endregion
    }
}
