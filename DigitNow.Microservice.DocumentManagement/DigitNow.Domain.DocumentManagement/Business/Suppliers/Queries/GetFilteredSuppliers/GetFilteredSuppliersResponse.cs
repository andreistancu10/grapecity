using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Suppliers.Queries.GetFilteredSuppliers
{
    internal class GetFilteredSuppliersResponse
    {
        public long TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long TotalPages { get; set; }
        public IList<SupplierViewModel> Items { get; set; }
    }
}
