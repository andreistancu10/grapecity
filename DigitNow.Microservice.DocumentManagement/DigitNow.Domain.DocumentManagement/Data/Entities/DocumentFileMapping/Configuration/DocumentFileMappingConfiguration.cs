using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class DocumentFileMappingConfiguration : IEntityTypeConfiguration<DocumentFileMapping>
    {
        public void Configure(EntityTypeBuilder<DocumentFileMapping> builder)
        {
            builder.ToTable(nameof(DocumentFileMapping), DocumentManagementDbContext.Schema);
            builder.HasKey(p => p.Id);
        }
    }
}
