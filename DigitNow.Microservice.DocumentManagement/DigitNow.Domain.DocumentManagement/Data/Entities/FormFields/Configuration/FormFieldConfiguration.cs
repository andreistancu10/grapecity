using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Configuration
{
    public class FormFieldConfiguration : IEntityTypeConfiguration<FormField>
    {
        public void Configure(EntityTypeBuilder<FormField> builder)
        {
            builder.ToTable(nameof(FormField), DocumentManagementDbContext.Schema);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasMaxLength(256).IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();

            builder.HasData(Seed.Data.GetFormFields());
        }
    }
}
