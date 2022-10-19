using DigitNow.Domain.DocumentManagement.Contracts.Suppliers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Suppliers
{
    public class SupplierFilter :DataFilter
    {
        public SupplierNameFilter SupplierNameFilter { get; set; }
        public SupplierCertificateRegistrationFilter SupplierCertificateRegistrationFilter { get; set; }
        public SupplierStatusFilter SupplierStatusFilter { get; set; }
        public static SupplierFilter Empty => new();
    }


    public class SupplierNameFilter
    {
        public string SupplierName { get; set; }
    }
    public class SupplierCertificateRegistrationFilter
    {
        public long CertificateRegistration { get; set; }
    }

    public class SupplierStatusFilter
    {
        public SupplierStatus SupplierStatus { get; set; }
    }
}
