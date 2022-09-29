using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class SpecialRegisterMappingConfiguration : IEntityTypeConfiguration<SpecialRegisterMapping>
    {
        public void Configure(EntityTypeBuilder<SpecialRegisterMapping> builder)
        {
            builder.ToTable(nameof(SpecialRegisterMapping), DocumentManagementDbContext.Schema);

            builder.HasKey(c => c.Id);

            builder.HasOne(item => item.Document)
                .WithMany(item => item.SpecialRegisterMappings)
                .HasForeignKey(x => x.DocumentId);
        }
    }
}