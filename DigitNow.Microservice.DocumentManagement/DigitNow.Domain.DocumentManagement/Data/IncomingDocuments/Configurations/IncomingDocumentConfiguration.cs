using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.Configurations
{
    public class IncomingDocumentConfiguration : IEntityTypeConfiguration<IncomingDocument>
    {
        public void Configure(EntityTypeBuilder<IncomingDocument> builder)
        {
            builder.ToTable(nameof(IncomingDocument), DocumentManagementDbContext.Schema);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.IssuerName).IsRequired();
            builder.Property(p => p.NumberOfPages).IsRequired();
            builder.Property(p => p.ContentSummary).IsRequired();
            builder.Property(p => p.User).IsRequired();
        }
    }
}
