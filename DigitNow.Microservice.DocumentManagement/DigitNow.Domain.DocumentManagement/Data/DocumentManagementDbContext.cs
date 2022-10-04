#define MIGRATION_ONLY

#if    MIGRATION_ONLY
using Microsoft.EntityFrameworkCore.Design;
#endif

using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.Actions;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Data
{
    public class DocumentManagementDbContext : DbContext
    {
        internal const string Schema = "DocumentManagement";

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
        public DbSet<SpecialRegister> SpecialRegisters { get; set; }
        public DbSet<SpecialRegisterMapping> SpecialRegisterMappings { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<UploadedFileMapping> UploadedFileMappings { get; set; }
        public DbSet<DocumentFileMapping> DocumentFileMappings { get; set; }
        public DbSet<DeliveryDetail> DeliveryDetails { get; set; }
        public DbSet<WorkflowHistoryLog> WorkflowHistoryLogs { get; set; }
        public DbSet<Objective> Objectives { get; set; }
        public DbSet<GeneralObjective> GeneralObjectives { get; set; }
        public DbSet<SpecificObjective> SpecificObjectives { get; set; }
        public DbSet<SpecificObjectiveFunctionary> SpecificObjectiveFunctionaries { get; set; }
        public DbSet<DocumentAction> DocumentActions { get; set; }
        public DbSet<DynamicForm> DynamicForms { get; set; }
        public DbSet<DynamicFormFieldMapping> DynamicFormFieldMappings { get; set; }
        public DbSet<DynamicFormFieldValue> DynamicFormFieldValues { get; set; }
        public DbSet<DynamicFormFillingLog> DynamicFormFillingLogs { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityFunctionary> ActivityFunctionaries { get; set; }
        public DbSet<Entities.Action> Actions { get; set; }
        public DbSet<ActionFunctionary> ActionFunctionaries { get; set; }
        public DbSet<Risk> Risks { get; set; }
        public DbSet<RiskControlAction> RiskControlActions { get; set; }
        public DbSet<RiskTrackingReport> RiskTrackingReports { get; set; }
        public DbSet<RiskActionProposal> RiskActionProposals{ get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<ProcedureFunctionary> ProcedureFunctionarys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocumentManagementDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<IExtendedEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        var currentUserId = _identityService.GetCurrentUserId();
                        entry.Entity.CreatedAt = DateTime.Now;
                        entry.Entity.CreatedBy = currentUserId;
                        entry.Entity.ModifiedAt = DateTime.Now;
                        entry.Entity.ModifiedBy = currentUserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedAt = DateTime.Now;
                        entry.Entity.ModifiedBy = _identityService.GetCurrentUserId();
                        break;
                }
            }

            foreach (var entry in ChangeTracker.Entries<ISoftExtendedEntity>())
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = DateTime.Now;
                    entry.Entity.DeletedBy = _identityService.GetCurrentUserId();
                }
            }

            foreach (var entry in ChangeTracker.Entries<IDocument>())
            {
                if (entry.Property(nameof(IDocument.Status)).IsModified)
                {
                    entry.Entity.StatusModifiedAt = DateTime.Now;
                    entry.Entity.StatusModifiedBy = _identityService.GetCurrentUserId();
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

                optionsBuilder.UseSqlServer("Server=localhost,1433;Database=DEV_DIGIT_NOW_DataKlas_DocumentManagement;User Id=sa;Password=admin123!;", builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });

                return new DocumentManagementDbContext(optionsBuilder.Options);
            }
        }
#endif
    }
}