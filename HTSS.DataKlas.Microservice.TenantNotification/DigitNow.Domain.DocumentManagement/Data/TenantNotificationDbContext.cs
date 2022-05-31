using Microsoft.EntityFrameworkCore;
using ShiftIn.Domain.TenantNotification.Data.Notifications;
using ShiftIn.Domain.TenantNotification.Data.NotificationStatuses;
using ShiftIn.Domain.TenantNotification.Data.NotificationTypeCoverGapExtensions;
using ShiftIn.Domain.TenantNotification.Data.NotificationTypes;

namespace ShiftIn.Domain.TenantNotification.Data
{
    public class TenantNotificationDbContext : DbContext
    {
        internal const string Schema = "tenantnotification";

        public TenantNotificationDbContext(DbContextOptions<TenantNotificationDbContext> options) : base(options)
        {
        }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<NotificationStatus> NotificationStatuses { get; set; }

        public DbSet<NotificationType> NotificationTypes { get; set; }

        public DbSet<NotificationTypeCoverGapExtension> NotificationTypeCoverGapExtensions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TenantNotificationDbContext).Assembly);
        }
    }
}