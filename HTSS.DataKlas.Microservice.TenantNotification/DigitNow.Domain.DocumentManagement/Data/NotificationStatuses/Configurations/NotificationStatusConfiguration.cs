using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShiftIn.Domain.TenantNotification.Data.NotificationStatuses.Configurations
{
    public class NotificationStatusConfiguration : IEntityTypeConfiguration<NotificationStatus>
    {
        public void Configure(EntityTypeBuilder<NotificationStatus> builder)
        {
            builder.ToTable(nameof(NotificationStatus), TenantNotificationDbContext.Schema);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).HasMaxLength(256).IsRequired();
            builder.Property(p => p.Code).HasMaxLength(256).IsRequired();
            builder.Property(p => p.CreatedBy);
            builder.Property(p => p.CreatedOn);
            builder.Property(p => p.ModifiedBy);
            builder.Property(p => p.ModifiedOn);
            builder.Property(p => p.Active);

            builder.HasIndex(p => p.Code).IsUnique();

            builder.HasData(Seed.Data.GetNotificationStatuses());
        }
    }
}