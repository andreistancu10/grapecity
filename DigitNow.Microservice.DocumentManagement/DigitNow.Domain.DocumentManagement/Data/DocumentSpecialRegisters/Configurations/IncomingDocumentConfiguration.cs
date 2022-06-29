using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.DocumentSpecialRegisters.Configurations;

public class DocumentSpecialRegisterConfiguration : IEntityTypeConfiguration<SpecialRegisterAssociation>
{
    public void Configure(EntityTypeBuilder<SpecialRegisterAssociation> builder)
    {
        builder.ToTable("DocumentSpecialRegisterAssociations", DocumentManagementDbContext.Schema);

        builder.HasKey(c => c.Id);
    }
}