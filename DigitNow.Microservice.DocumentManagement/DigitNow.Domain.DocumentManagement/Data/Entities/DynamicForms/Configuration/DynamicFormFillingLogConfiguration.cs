using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.DynamicForms.Configuration
{
    public class DynamicFormFillingLogConfiguration : IEntityTypeConfiguration<DynamicFormFillingLog>
    {
        public void Configure(EntityTypeBuilder<DynamicFormFillingLog> builder)
        {
            builder.ToTable(nameof(DynamicFormFillingLog), DocumentManagementDbContext.Schema);
            builder.HasKey(p => p.Id);

            builder.HasOne(c => c.DynamicForm)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
