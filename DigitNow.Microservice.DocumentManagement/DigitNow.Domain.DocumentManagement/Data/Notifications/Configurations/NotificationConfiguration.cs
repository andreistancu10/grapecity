using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Notifications.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable(nameof(Notification), DocumentManagementDbContext.Schema);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Message).HasMaxLength(2048).IsRequired();
            builder.Property(p => p.NotificationTypeId);
            builder.Property(p => p.NotificationStatusId);
            builder.Property(p => p.UserId);
            builder.Property(p => p.FromUserId);
            builder.Property(p => p.EntityId);
            builder.Property(p => p.EntityTypeId);
            builder.Property(p => p.Seen);
            builder.Property(p => p.SeenOn);
            builder.Property(p => p.CreatedOn);
            builder.Property(p => p.ModifiedOn);
        }
    }
}