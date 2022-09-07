using DigitNow.Domain.DocumentManagement.Data.Entities.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Configuration
{
    public class DynamicFormConfiguration : IEntityTypeConfiguration<DynamicForm>
    {
        public void Configure(EntityTypeBuilder<DynamicForm> builder)
        {
            builder.ToTable(nameof(DynamicForm), DocumentManagementDbContext.Schema);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasMaxLength(256).IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();

            builder.HasData(DynamicFormData.GetForms());                       
        }
    }
}
