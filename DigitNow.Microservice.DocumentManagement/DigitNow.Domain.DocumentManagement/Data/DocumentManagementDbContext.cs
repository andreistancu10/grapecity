using DigitNow.Domain.DocumentManagement.Data.Notifications;
using DigitNow.Domain.DocumentManagement.Data.NotificationStatuses;
using DigitNow.Domain.DocumentManagement.Data.NotificationTypeCoverGapExtensions;
using DigitNow.Domain.DocumentManagement.Data.NotificationTypes;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Data
{
    public class DocumentManagementDbContext : DbContext
    {
        internal const string Schema = "documentamangement";

        public DocumentManagementDbContext(DbContextOptions<DocumentManagementDbContext> options) : base(options)
        {
        }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<NotificationStatus> NotificationStatuses { get; set; }

        public DbSet<NotificationType> NotificationTypes { get; set; }

        public DbSet<NotificationTypeCoverGapExtension> NotificationTypeCoverGapExtensions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocumentManagementDbContext).Assembly);
        }
    }
}