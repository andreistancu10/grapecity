using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisters.Configurations;

public class SpecialRegisterConfiguration : IEntityTypeConfiguration<SpecialRegister>
{
    public void Configure(EntityTypeBuilder<SpecialRegister> builder)
    {
        builder.ToTable(nameof(SpecialRegister), DocumentManagementDbContext.Schema);

        builder.HasKey(p => p.Id);
    }
}