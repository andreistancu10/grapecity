using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class DeliveryDetailConfiguration : IEntityTypeConfiguration<DeliveryDetail>
    {
        public void Configure(EntityTypeBuilder<DeliveryDetail> builder)
        {
            builder.ToTable(nameof(DeliveryDetail), DocumentManagementDbContext.Schema);
            builder.HasKey(p => p.Id);

            builder.Property(p => p.DeliveryMode).IsRequired();
        }
    }
}
