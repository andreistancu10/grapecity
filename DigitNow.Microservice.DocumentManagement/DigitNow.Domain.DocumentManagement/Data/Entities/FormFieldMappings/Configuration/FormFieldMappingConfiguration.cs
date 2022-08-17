using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Configuration
{
    public class FormFieldMappingConfiguration : IEntityTypeConfiguration<FormFieldMapping>
    {
        public void Configure(EntityTypeBuilder<FormFieldMapping> builder)
        {
            builder.ToTable(nameof(FormFieldMapping), DocumentManagementDbContext.Schema);
            builder.HasKey(p => p.Id);

            builder.HasData(Seed.Data.GetFormFieldMappings());
        }
    }
}
