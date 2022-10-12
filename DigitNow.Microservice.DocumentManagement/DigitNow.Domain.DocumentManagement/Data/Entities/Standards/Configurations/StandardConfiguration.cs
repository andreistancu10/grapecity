using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Standards.Configurations
{
    public class StandardConfiguration : IEntityTypeConfiguration<Standard>
    {
        public void Configure(EntityTypeBuilder<Standard> builder)
        {
            builder.ToTable(nameof(Standard), DocumentManagementDbContext.Schema);

            builder.Property(p => p.DepartmentId).IsRequired();
        }
    }
}
