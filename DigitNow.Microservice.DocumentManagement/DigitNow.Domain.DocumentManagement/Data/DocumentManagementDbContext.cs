using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;
using Microsoft.EntityFrameworkCore;
using DigitNow.Domain.DocumentManagement.Data.ConnectedDocuments;
using DigitNow.Domain.DocumentManagement.Data.ContactDetails;

namespace DigitNow.Domain.DocumentManagement.Data
{
    public class DocumentManagementDbContext : DbContext
    {
        internal const string Schema = "documentamangement";

        public DocumentManagementDbContext(DbContextOptions<DocumentManagementDbContext> options) : base(options)
        {
        }

        public DbSet<IncomingDocument> IncomingDocuments { get; set; }

        public DbSet<ConnectedDocument> ConnectedDocuments { get; set; }

        public DbSet<ContactDetail> ContactDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocumentManagementDbContext).Assembly);
        }
    }
}