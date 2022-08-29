using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class UploadedFileConfiguration : IEntityTypeConfiguration<UploadedFile>
    {
        public void Configure(EntityTypeBuilder<UploadedFile> builder)
        {
            builder.ToTable(nameof(UploadedFile), DocumentManagementDbContext.Schema);
            builder.HasKey(p => p.Id);

            builder
                .HasOne(c => c.UploadedFileMapping)
                .WithOne(c => c.UploadedFile)
                .HasForeignKey<UploadedFileMapping>();
        }
    }
}