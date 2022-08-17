using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Configuration
{
    public class FormConfiguration : IEntityTypeConfiguration<Form>
    {
        public void Configure(EntityTypeBuilder<Form> builder)
        {
            builder.ToTable(nameof(Form), DocumentManagementDbContext.Schema);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasMaxLength(256).IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();

            builder.HasData(Seed.Data.GetForms());
        }
    }
}
