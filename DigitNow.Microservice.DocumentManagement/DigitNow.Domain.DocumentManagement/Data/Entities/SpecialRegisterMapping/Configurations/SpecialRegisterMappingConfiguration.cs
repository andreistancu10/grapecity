using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisterMappings.Configurations;

public class SpecialRegisterMappingConfiguration : IEntityTypeConfiguration<SpecialRegisterMapping>
{
    public void Configure(EntityTypeBuilder<SpecialRegisterMapping> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne(item => item.Document)
            .WithMany(item => item.SpecialRegisterMappings)
            .HasForeignKey(x => x.DocumentId);
    }
}