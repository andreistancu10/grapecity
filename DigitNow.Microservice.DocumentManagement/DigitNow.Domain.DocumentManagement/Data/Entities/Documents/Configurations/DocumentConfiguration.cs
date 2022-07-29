using DigitNow.Domain.DocumentManagement.Data.Entities.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable(nameof(Document), DocumentManagementDbContext.Schema);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.DocumentType).IsRequired();
        builder.Property(p => p.RegistrationNumber).IsRequired();
        builder.Property(p => p.RegistrationDate).IsRequired();
    }
}