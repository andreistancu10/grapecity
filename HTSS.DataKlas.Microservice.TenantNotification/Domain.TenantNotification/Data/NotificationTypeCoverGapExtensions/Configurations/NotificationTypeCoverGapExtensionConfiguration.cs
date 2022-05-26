using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShiftIn.Domain.TenantNotification.Data.NotificationTypeCoverGapExtensions.Configurations
{
    public class NotificationTypeCoverGapExtensionConfiguration : IEntityTypeConfiguration<NotificationTypeCoverGapExtension>
    {
        public void Configure(EntityTypeBuilder<NotificationTypeCoverGapExtension> builder)
        {
            builder.ToTable(nameof(NotificationTypeCoverGapExtension), TenantNotificationDbContext.Schema);

            builder.HasKey(p => p.Id);
            builder.Property(p => p.NotificationTypeId);
            builder.Property(p => p.CreatedBy);
            builder.Property(p => p.CreatedOn);
            builder.Property(p => p.ModifiedBy);
            builder.Property(p => p.ModifiedOn);
            builder.Property(p => p.Active);

            builder.HasOne(p => p.NotificationType).WithMany().HasForeignKey(p => p.NotificationTypeId);
        }
    }
}