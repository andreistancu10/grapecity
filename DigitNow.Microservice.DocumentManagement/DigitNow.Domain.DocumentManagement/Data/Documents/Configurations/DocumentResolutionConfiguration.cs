using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Documents.Configurations;

public class DocumentResolutionConfiguration : IEntityTypeConfiguration<DocumentResolution>
{
    public void Configure(EntityTypeBuilder<DocumentResolution> builder)
    {
        builder.ToTable(nameof(DocumentResolution), DocumentManagementDbContext.Schema);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.DocumentId).IsRequired();
        builder.Property(p => p.DocumentType).IsRequired();
        builder.Property(p => p.ResolutionType).IsRequired();
        builder.Property(p => p.Remarks).IsRequired();
    }
}