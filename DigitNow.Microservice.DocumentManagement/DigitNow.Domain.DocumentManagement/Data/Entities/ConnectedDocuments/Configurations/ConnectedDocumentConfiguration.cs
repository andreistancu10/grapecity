using DigitNow.Domain.DocumentManagement.Data.Entities.ConnectedDocuments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities;

public class ConnectedDocumentConfiguration : IEntityTypeConfiguration<ConnectedDocument>
{
    public void Configure(EntityTypeBuilder<ConnectedDocument> builder)
    {
        builder.ToTable(nameof(ConnectedDocument), DocumentManagementDbContext.Schema);

        builder.HasKey(p => p.Id);
    }
}