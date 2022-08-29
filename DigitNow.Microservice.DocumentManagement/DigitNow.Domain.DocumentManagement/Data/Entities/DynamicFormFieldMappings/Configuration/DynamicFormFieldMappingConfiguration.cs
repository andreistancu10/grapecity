using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Configuration
{
    public class DynamicFormFieldMappingConfiguration : IEntityTypeConfiguration<DynamicFormFieldMapping>
    {
        public void Configure(EntityTypeBuilder<DynamicFormFieldMapping> builder)
        {
            builder.ToTable(nameof(DynamicFormFieldMapping), DocumentManagementDbContext.Schema);
            builder.HasKey(p => p.Id);

            builder.HasData(Seed.Data.GetFormFieldMappings());
        }
    }
}
