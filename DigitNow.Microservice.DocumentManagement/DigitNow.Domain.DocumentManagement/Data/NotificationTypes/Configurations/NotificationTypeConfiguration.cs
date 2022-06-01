using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.NotificationTypes.Configurations
{
    public class NotificationTypeConfiguration : IEntityTypeConfiguration<NotificationType>
    {
        public void Configure(EntityTypeBuilder<NotificationType> builder)
        {
            builder.ToTable(nameof(NotificationType), DocumentManagementDbContext.Schema);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).HasMaxLength(256).IsRequired();
            builder.Property(p => p.Code).HasMaxLength(256).IsRequired();
            builder.Property(p => p.EntityType).HasMaxLength(256).IsRequired();
            builder.Property(p => p.TranslationLabel);
            builder.Property(p => p.Expression);
            builder.Property(p => p.IsInformative);
            builder.Property(p => p.IsUrgent);
            builder.Property(p => p.Active);
            builder.Property(p => p.CreatedBy);
            builder.Property(p => p.CreatedOn);
            builder.Property(p => p.ModifiedBy);
            builder.Property(p => p.ModifiedOn);
            builder.Property(p => p.NotificationStatusId);

            builder.HasOne(p => p.NotificationStatus).WithMany().HasForeignKey(p => p.NotificationStatusId);

            builder.HasIndex(p => p.Code).IsUnique();

            builder.HasData(Seed.Data.GetNotificationTypes());
        }
    }
}