using DigitNow.Domain.DocumentManagement.Data.Notifications;
using DigitNow.Domain.DocumentManagement.Data.NotificationStatuses;
using DigitNow.Domain.DocumentManagement.Data.NotificationTypeCoverGapExtensions;
using DigitNow.Domain.DocumentManagement.Data.NotificationTypes;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;
using Microsoft.EntityFrameworkCore;
using DigitNow.Domain.DocumentManagement.Data.ConnectedDocuments;
using DigitNow.Domain.DocumentManagement.Data.ContactDetails;
using DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments;

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

        public DbSet<IncomingDocument> IncomingDocuments { get; set; }

        public DbSet<OutgoingDocument> OutgoingDocuments { get; set; }

        public DbSet<ConnectedDocument> ConnectedDocuments { get; set; }

        public DbSet<ContactDetail> ContactDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocumentManagementDbContext).Assembly);
        }
    }
}