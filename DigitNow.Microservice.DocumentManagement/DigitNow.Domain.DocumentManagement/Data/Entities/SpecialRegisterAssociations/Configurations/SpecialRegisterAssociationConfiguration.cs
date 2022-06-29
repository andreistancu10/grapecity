using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisterAssociations.Configurations;

public class SpecialRegisterAssociationConfiguration : IEntityTypeConfiguration<SpecialRegisterAssociation>
{
    public void Configure(EntityTypeBuilder<SpecialRegisterAssociation> builder)
    {
        builder.HasKey(c => c.Id);
    }
}