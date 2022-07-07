using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.DocumentUploadedFiles.Configurations;

public class DocumentUploadedFileConfiguration : IEntityTypeConfiguration<DocumentUploadedFile>
{
    public void Configure(EntityTypeBuilder<DocumentUploadedFile> builder)
    {
        builder.ToTable(nameof(DocumentUploadedFile), DocumentManagementDbContext.Schema);
        builder.HasKey(p => p.Id);
    }
}