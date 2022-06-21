using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DigitNow.Domain.DocumentManagement.Data.ConnectedDocuments;
using DigitNow.Domain.DocumentManagement.Data.Documents;
using DigitNow.Domain.DocumentManagement.Data.RegistrationNumberCounters;

namespace DigitNow.Domain.DocumentManagement.Data
{
    public class DocumentManagementDbContext : DbContext
    {
        internal const string Schema = "DocumentMangement";

        public DocumentManagementDbContext(DbContextOptions<DocumentManagementDbContext> options) : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; }

        public DbSet<IncomingDocument> IncomingDocuments { get; set; }

        public DbSet<OutgoingDocument> OutgoingDocuments { get; set; }        
        
        public DbSet<InternalDocument> InternalDocuments { get; set; }

        public DbSet<DocumentResolution> DocumentResolutions { get; set; }

        public DbSet<ConnectedDocument> ConnectedDocuments { get; set; }

        public DbSet<RegistrationNumberCounter> RegistrationNumberCounters { get; set; }

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