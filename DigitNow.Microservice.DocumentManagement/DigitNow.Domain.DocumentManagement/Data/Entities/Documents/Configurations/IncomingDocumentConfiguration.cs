using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities;

public class IncomingDocumentConfiguration : IEntityTypeConfiguration<IncomingDocument>
{
    public void Configure(EntityTypeBuilder<IncomingDocument> builder)
    {
        builder.ToTable(nameof(IncomingDocument), DocumentManagementDbContext.Schema);

        builder.Property(p => p.IssuerName).IsRequired();
        builder.Property(p => p.RegistrationNumber).IsRequired();
        builder.Property(p => p.NumberOfPages).IsRequired();
        builder.Property(p => p.ContentSummary).IsRequired();

        builder.HasOne(item => item.Document)
            .WithOne(item => item.IncomingDocument)
            .HasForeignKey<IncomingDocument>(item => item.DocumentId);

        builder.HasMany(item => item.ConnectedDocuments)
            .WithOne(item => item.IncomingDocument)
            .HasForeignKey(item => item.Id)
            .OnDelete(DeleteBehavior.NoAction);
    }
}