using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities;

public class InternalDocumentConfiguration : IEntityTypeConfiguration<InternalDocument>
{
    public void Configure(EntityTypeBuilder<InternalDocument> builder)
    {
        builder.ToTable(nameof(InternalDocument), DocumentManagementDbContext.Schema);

        builder.Property(p => p.RegistrationNumber).IsRequired();
        builder.Property(p => p.DepartmentId).IsRequired();
        builder.Property(p => p.InternalDocumentTypeId).IsRequired();
        builder.Property(p => p.DeadlineDaysNumber).IsRequired();
        builder.Property(p => p.Description).IsRequired();
        builder.Property(p => p.Observation);
        builder.Property(p => p.ReceiverDepartmentId).IsRequired();
        builder.Property(p => p.IsUrgent);

        builder.HasOne(item => item.Document)
            .WithOne(item => item.InternalDocument)
            .HasForeignKey<InternalDocument>(item => item.DocumentId);
    }
}