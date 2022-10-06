using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Configuration
{
    public class DynamicFormFieldValueConfiguration : IEntityTypeConfiguration<DynamicFormFieldValue>
    {
        public void Configure(EntityTypeBuilder<DynamicFormFieldValue> builder)
        {
            builder.ToTable(nameof(DynamicFormFieldValue), DocumentManagementDbContext.Schema);
            builder.HasKey(p => p.Id);

            builder.HasOne(item => item.DynamicFormFieldMapping)
                .WithMany(item => item.DynamicFormFieldValues)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(item => item.DynamicFormFieldMappingId);

            builder.HasOne(c => c.DynamicFormFillingLog)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
