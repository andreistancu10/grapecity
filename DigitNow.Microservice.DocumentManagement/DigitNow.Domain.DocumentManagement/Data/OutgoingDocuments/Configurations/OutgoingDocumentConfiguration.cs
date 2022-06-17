using DigitNow.Domain.DocumentManagement.Data.OutgoingConnectedDocuments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments.Configurations;

public class OutgoingConnectedDocumentConfiguration : IEntityTypeConfiguration<OutgoingConnectedDocument>
{
    public void Configure(EntityTypeBuilder<OutgoingConnectedDocument> builder)
    {
        builder.ToTable(nameof(OutgoingConnectedDocument), DocumentManagementDbContext.Schema);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.RegistrationNumber).IsRequired();
        builder.Property(p => p.DocumentType).IsRequired();
    }
}