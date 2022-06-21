using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.ConnectedDocuments.Configurations;

public class ConnectedDocumentConfiguration : IEntityTypeConfiguration<ConnectedDocument>
{
    public void Configure(EntityTypeBuilder<ConnectedDocument> builder)
    {
        builder.ToTable(nameof(ConnectedDocument), DocumentManagementDbContext.Schema);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.RegistrationNumber).IsRequired();
        builder.Property(p => p.DocumentType).IsRequired();
    }
}