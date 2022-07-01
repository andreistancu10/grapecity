using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisterMapping.Configurations;

public class SpecialRegisterMappingConfiguration : IEntityTypeConfiguration<SpecialRegisterMapping>
{
    public void Configure(EntityTypeBuilder<SpecialRegisterMapping> builder)
    {
        builder.HasKey(c => c.Id);
    }
}