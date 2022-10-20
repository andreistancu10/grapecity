using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Abstractions;
using DigitNow.Domain.DocumentManagement.Data.Filters.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Suppliers
{
    internal class SupplierFilterComponentContext : IDataExpressionFilterComponentContext
    {
        public SupplierFilter SupplierFilter { get; set; }
    }

}
