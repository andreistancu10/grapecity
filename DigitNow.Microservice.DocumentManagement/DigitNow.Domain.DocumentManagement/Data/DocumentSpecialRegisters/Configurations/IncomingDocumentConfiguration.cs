using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.DocumentSpecialRegisters.Configurations;

public class DocumentSpecialRegisterConfiguration : IEntityTypeConfiguration<DocumentSpecialRegister>
{
    public void Configure(EntityTypeBuilder<DocumentSpecialRegister> builder)
    {
        builder.ToTable("DocumentSpecialRegisterAssociations", DocumentManagementDbContext.Schema);

        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.Id);
    }
}