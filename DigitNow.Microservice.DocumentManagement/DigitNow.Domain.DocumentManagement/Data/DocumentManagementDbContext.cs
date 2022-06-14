using DigitNow.Domain.DocumentManagement.Data.Notifications;
using DigitNow.Domain.DocumentManagement.Data.NotificationStatuses;
using DigitNow.Domain.DocumentManagement.Data.NotificationTypeCoverGapExtensions;
using DigitNow.Domain.DocumentManagement.Data.NotificationTypes;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;
using Microsoft.EntityFrameworkCore;
using DigitNow.Domain.DocumentManagement.Data.ContactDetails;
using DigitNow.Domain.DocumentManagement.Data.IncomingConnectedDocuments;
using DigitNow.Domain.DocumentManagement.Data.OutgoingConnectedDocuments;
using DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments;

namespace DigitNow.Domain.DocumentManagement.Data
{
    public class DocumentManagementDbContext : DbContext
    {
        internal const string Schema = "DocumentMangement";

        public DocumentManagementDbContext(DbContextOptions<DocumentManagementDbContext> options) : base(options)
        {
        }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<NotificationStatus> NotificationStatuses { get; set; }

        public DbSet<NotificationType> NotificationTypes { get; set; }

        public DbSet<NotificationTypeCoverGapExtension> NotificationTypeCoverGapExtensions { get; set; }

        public DbSet<IncomingDocument> IncomingDocuments { get; set; }

        public DbSet<OutgoingDocument> OutgoingDocuments { get; set; }

        public DbSet<IncomingConnectedDocument> IncomingConnectedDocuments { get; set; }

        public DbSet<OutgoingConnectedDocument> OutgoingConnectedDocuments { get; set; }
        
        public DbSet<InternalDocument.InternalDocument> InternalDocuments { get; set; }

        public DbSet<ConnectedDocument> ConnectedDocuments { get; set; }

        public DbSet<RegistrationNoCounter.RegistrationNumberCounter> RegistrationNumberCounter { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocumentManagementDbContext).Assembly);
        }
        
        public class DbContextFactory : IDesignTimeDbContextFactory<DocumentManagementDbContext>
        {
            public DocumentManagementDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<DocumentManagementDbContext>();

                optionsBuilder.UseSqlServer("Server=localhost,1433;Database=DigitNow-dev-DocumentManagement;User Id=sa;Password=admin123!;", builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
                
                return new DocumentManagementDbContext(optionsBuilder.Options);
            }
        }
    }
}