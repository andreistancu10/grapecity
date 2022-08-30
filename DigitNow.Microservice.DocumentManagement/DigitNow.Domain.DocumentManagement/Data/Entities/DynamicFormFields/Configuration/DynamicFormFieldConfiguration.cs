using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Configuration
{
    public class DynamicFormFieldConfiguration : IEntityTypeConfiguration<DynamicFormField>
    {
        public void Configure(EntityTypeBuilder<DynamicFormField> builder)
        {
            builder.ToTable(nameof(DynamicFormField), DocumentManagementDbContext.Schema);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasMaxLength(256).IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();

            builder.HasData(Seed.Data.GetFormFields());
        }
    }
}
