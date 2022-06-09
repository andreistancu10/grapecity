using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.IncomingConnectedDocuments.Configurations;

public class IncomingConnectedDocumentConfiguration : IEntityTypeConfiguration<IncomingConnectedDocument>
{
    public void Configure(EntityTypeBuilder<IncomingConnectedDocument> builder)
    {
        builder.ToTable(nameof(IncomingConnectedDocument), DocumentManagementDbContext.Schema);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.RegistrationNumber).IsRequired();
        builder.Property(p => p.DocumentType).IsRequired();
    }
}