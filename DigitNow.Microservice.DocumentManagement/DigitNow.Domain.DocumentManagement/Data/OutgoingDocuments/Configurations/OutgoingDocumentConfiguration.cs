using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments.Configurations;

public class OutgoingDocumentConfiguration : IEntityTypeConfiguration<OutgoingDocument>
{
    public void Configure(EntityTypeBuilder<OutgoingDocument> builder)
    {
        builder.ToTable(nameof(OutgoingDocument), DocumentManagementDbContext.Schema);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.RegistrationNumber).IsRequired();
        builder.Property(p => p.RecipientId).IsRequired();
        builder.Property(p => p.ContentSummary).IsRequired();
        builder.Property(p => p.DocumentTypeId).IsRequired();
        builder.Property(p => p.NumberOfPages).IsRequired();
        builder.Property(p => p.User).IsRequired();
    }
}