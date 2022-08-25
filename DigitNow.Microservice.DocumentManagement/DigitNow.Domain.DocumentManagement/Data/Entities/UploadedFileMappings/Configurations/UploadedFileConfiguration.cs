using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class DocumentUploadedFileConfiguration : IEntityTypeConfiguration<UploadedFileMapping>
    {
        public void Configure(EntityTypeBuilder<UploadedFileMapping> builder)
        {
            builder.ToTable(nameof(UploadedFileMapping), DocumentManagementDbContext.Schema);
            builder.HasKey(p => p.Id);
        }
    }
}