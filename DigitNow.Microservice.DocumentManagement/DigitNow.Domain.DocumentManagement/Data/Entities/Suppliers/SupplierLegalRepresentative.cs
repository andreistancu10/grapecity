using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Suppliers
{
    public class SupplierLegalRepresentative : ExtendedEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string RepresentativeQuality { get; set; }
        public string NationalId { get; set; }
        public long SupplierId { get; set; }
        public Supplier Supplier { get; set; } 


    }
}
