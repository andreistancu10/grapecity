using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.DeliveryDetails.Configurations
{
    public class DeliveryDetailConfiguration
    {
        public void Configure(EntityTypeBuilder<DeliveryDetail> builder)
        {
            builder.ToTable(nameof(DeliveryDetail), DocumentManagementDbContext.Schema);
            builder.HasKey(p => p.Id);

            builder.Property(p => p.DeliveryMode).IsRequired();
        }
    }
}
