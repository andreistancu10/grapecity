using DigitNow.Domain.DocumentManagement.Contracts.Suppliers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Public.Suppliers.Models
{
    public class GetFilteredSupplierRequest
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public SupplierFilterDto Filter { get; set; }
    }

    public class SupplierFilterDto 
    {
        public SupplierNameFilterDto SupplierNameFilter { get; set; }
        public SupplierCertificateRegistrationFilterDto SupplierCertificateRegistrationFilter { get; set; }
        public SupplierStatusFilterDto SupplierStatusFilter { get; set; }
     
    }


    public class SupplierNameFilterDto
    {
        public string SupplierName { get; set; }
    }
    public class SupplierCertificateRegistrationFilterDto
    {
        public long CertificateRegistration { get; set; }
    }

    public class SupplierStatusFilterDto
    {
        public SupplierStatus SupplierStatus { get; set; }
    }
}
