using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Objectives.Configurations
{
    public class BasicUploadedFileConfiguration : IEntityTypeConfiguration<BasicUploadedFile>
    {
        public void Configure(EntityTypeBuilder<BasicUploadedFile> builder)
        {
            builder.ToTable(nameof(BasicUploadedFile), DocumentManagementDbContext.Schema);
            builder.HasKey(p => p.Id);
        }
    }
}
