using DigitNow.Domain.DocumentManagement.Contracts.Suppliers.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Suppliers.Configurations
{
    public class SupplierConfigurations : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable(nameof(Supplier), DocumentManagementDbContext.Schema);

            builder.Property(p => p.Status).IsRequired().HasDefaultValue(SupplierStatus.Active);
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.CertificateRegistration).IsRequired();
            builder.Property(p => p.VatPayer).IsRequired();
            builder.Property(p => p.CompanyType).IsRequired();
            builder.Property(p => p.RegisteredWorkplace).IsRequired();

                      
        }
    }
}
