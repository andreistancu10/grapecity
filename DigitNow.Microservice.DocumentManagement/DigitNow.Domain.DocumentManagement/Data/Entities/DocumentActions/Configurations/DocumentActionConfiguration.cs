using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.DocumentActions.Configurations
{
    public class DocumentActionConfiguration : IEntityTypeConfiguration<DocumentAction>
    {
        public void Configure(EntityTypeBuilder<DocumentAction> builder)
        {
            builder.ToTable(nameof(DocumentAction), DocumentManagementDbContext.Schema);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.DocumentId).IsRequired();
            builder.Property(p => p.Action).IsRequired();
            builder.Property(p => p.ResposibleId).IsRequired();

            builder.HasOne(item => item.Document)
                .WithMany(item => item.DocumentActions)
                .HasForeignKey(item => item.DocumentId);
        }
    }
}
