using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Configuration
{
    public class FormFieldValueConfiguration : IEntityTypeConfiguration<FormFieldValue>
    {
        public void Configure(EntityTypeBuilder<FormFieldValue> builder)
        {
            builder.ToTable(nameof(FormFieldValue), DocumentManagementDbContext.Schema);
            builder.HasKey(p => p.Id);

            builder.HasOne(c => c.FormFieldMapping)
                .WithMany()
                //.HasForeignKey(c => c.FormFieldMapping)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.FormFillingLog)
                .WithMany()
                //.HasForeignKey(c => c.FormFieldMapping)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
