using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Objectives.Configurations
{
    public class ObjectiveUploadedFileConfiguration : IEntityTypeConfiguration<ObjectiveUploadedFile>
    {
        public void Configure(EntityTypeBuilder<ObjectiveUploadedFile> builder)
        {
            builder.ToTable(nameof(ObjectiveUploadedFile), DocumentManagementDbContext.Schema);
            builder.HasKey(p => p.Id);
        }
    }
}