using DigitNow.Domain.DocumentManagement.Data.Entities.Suppliers;
using DigitNow.Domain.DocumentManagement.Data.Filters.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Suppliers
{
    internal class SupplierFilterBuilder :  DataExpressionFilterBuilder<Supplier, SupplierFilter>
    {
        public SupplierFilterBuilder(IServiceProvider serviceProvider, SupplierFilter filter) : base(serviceProvider, filter) { }

        protected override void InternalBuild()
        {
            BuildFilterBySupplierName();
            BuildFilterBySupplierStatus();
            BuildFilterBySupplierCertificateRegistration();
        }

        private void BuildFilterBySupplierName()
        {
            if (EntityFilter.SupplierNameFilter != null)
            {
                EntityPredicates.Add(x => x.Name.Equals(EntityFilter.SupplierNameFilter.SupplierName));
            }
        }
        private void BuildFilterBySupplierStatus()
        {
            if (EntityFilter.SupplierStatusFilter != null)
            {
                EntityPredicates.Add(x => x.Status.Equals(EntityFilter.SupplierStatusFilter.SupplierStatus));
            }
        }
        private void BuildFilterBySupplierCertificateRegistration()
        {
            if (EntityFilter.SupplierCertificateRegistrationFilter != null)
            {
                EntityPredicates.Add(x => x.CertificateRegistration.Equals(EntityFilter.SupplierCertificateRegistrationFilter.CertificateRegistration));
            }
        }
    }
}
