﻿#define MIGRATION_ONLY

using System;
using Microsoft.EntityFrameworkCore;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using System.Threading.Tasks;
using System.Threading;

#if  MIGRATION_ONLY
using Microsoft.EntityFrameworkCore.Design;
#endif
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisterMapping;
using DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisters;

namespace DigitNow.Domain.DocumentManagement.Data
{
    public class DocumentManagementDbContext : DbContext
    {
        internal const string Schema = "DocumentMangement";

        private readonly IIdentityService _identityService;

        public DocumentManagementDbContext(DbContextOptions<DocumentManagementDbContext> options)
            : base(options) { }

        public DocumentManagementDbContext(DbContextOptions<DocumentManagementDbContext> options,
            IIdentityService identityService) 
            : base(options)
        {
            _identityService = identityService;
        }
        public DbSet<Document> Documents { get; set; }
        public DbSet<IncomingDocument> IncomingDocuments { get; set; }
        public DbSet<OutgoingDocument> OutgoingDocuments { get; set; }
        public DbSet<DocumentResolution> DocumentResolutions { get; set; }
        public DbSet<ConnectedDocument> ConnectedDocuments { get; set; }
        public DbSet<InternalDocument> InternalDocuments { get; set; }
        public DbSet<RegistrationNumberCounter> RegistrationNumberCounters { get; set; }
        public DbSet<SpecialRegister> SpecialRegisters { get; set; }
        public DbSet<SpecialRegisterMapping> SpecialRegisterMappings { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocumentManagementDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<IExtendedEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.Now;
                        entry.Entity.CreatedBy = _identityService.GetCurrentUserId();
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedAt = DateTime.Now;
                        entry.Entity.ModifiedBy = _identityService.GetCurrentUserId();
                        break;
                }
            }

            foreach (var entry in ChangeTracker.Entries<ISoftExtendedEntity>())
            {
                if(entry.State == EntityState.Deleted)
                {
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = DateTime.Now;
                    entry.Entity.DeletedBy = _identityService.GetCurrentUserId();
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

#if MIGRATION_ONLY
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
#endif
    }
}