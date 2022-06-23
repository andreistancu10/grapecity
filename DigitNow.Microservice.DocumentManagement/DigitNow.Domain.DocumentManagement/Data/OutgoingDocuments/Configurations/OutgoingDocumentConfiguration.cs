using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments.Configurations;

public class OutgoingConnectedDocumentConfiguration : IEntityTypeConfiguration<OutgoingDocument>
{
    public void Configure(EntityTypeBuilder<OutgoingDocument> builder)
    {
        builder.ToTable(nameof(OutgoingDocument), DocumentManagementDbContext.Schema);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.RegistrationNumber).IsRequired();
    }
}