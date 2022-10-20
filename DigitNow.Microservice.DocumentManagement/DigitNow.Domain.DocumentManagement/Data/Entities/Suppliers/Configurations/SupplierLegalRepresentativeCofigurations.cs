using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Suppliers.Configurations
{
    public class SupplierLegalRepresentativeCofigurations : IEntityTypeConfiguration<SupplierLegalRepresentative>
    {
        public void Configure(EntityTypeBuilder<SupplierLegalRepresentative> builder)
        {
            builder.ToTable(nameof(SupplierLegalRepresentative), DocumentManagementDbContext.Schema);

            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Surname).IsRequired();
            builder.Property(p => p.RepresentativeQuality).IsRequired();
            builder.Property(p => p.NationalId).IsRequired();
            builder.Property(p => p.SupplierId).IsRequired();

            builder.HasOne(item => item.Supplier)
             .WithMany(item => item.LegalRepresentatives)
             .HasForeignKey(item => item.SupplierId);

        }
    }
}
