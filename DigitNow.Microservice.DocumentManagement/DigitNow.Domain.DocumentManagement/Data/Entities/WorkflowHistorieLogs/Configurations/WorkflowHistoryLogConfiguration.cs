using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class WorkflowHistoryLogConfiguration : IEntityTypeConfiguration<WorkflowHistoryLog>
    {
        public void Configure(EntityTypeBuilder<WorkflowHistoryLog> builder)
        {
            builder.ToTable(nameof(WorkflowHistoryLog), DocumentManagementDbContext.Schema);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.DocumentId).IsRequired();
            builder.Property(p => p.DocumentStatus).IsRequired();
            builder.Property(p => p.RecipientId).IsRequired();
            builder.Property(p => p.RecipientType).IsRequired();

            builder.HasOne(item => item.Document)
                .WithMany(item => item.WorkflowHistories)            
                .HasForeignKey(item => item.DocumentId);
        }
    }
}