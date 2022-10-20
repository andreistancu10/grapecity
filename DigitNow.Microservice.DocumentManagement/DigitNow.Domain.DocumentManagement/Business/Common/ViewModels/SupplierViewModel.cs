using DigitNow.Domain.DocumentManagement.Contracts.Suppliers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class SupplierViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public long CertificateRegistration { get; set; }

    }
}
