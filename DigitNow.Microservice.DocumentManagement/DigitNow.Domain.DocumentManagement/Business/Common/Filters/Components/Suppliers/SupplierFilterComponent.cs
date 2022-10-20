using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.Suppliers;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Suppliers
{
    internal class SupplierFilterComponent : DataExpressionFilterComponent<Supplier, SupplierFilterComponentContext>
    {
        public SupplierFilterComponent(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Task<DataExpressions<Supplier>> SetCustomDataExpressionsAsync(SupplierFilterComponentContext context, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<Supplier>();
            if (!context.SupplierFilter.IsEmpty())
            {
                var filterBuilder = new SupplierFilterBuilder(ServiceProvider, context.SupplierFilter);
                dataExpressions.AddRange(filterBuilder.Build());
            }

            return Task.FromResult(dataExpressions);
        }
    }
}
