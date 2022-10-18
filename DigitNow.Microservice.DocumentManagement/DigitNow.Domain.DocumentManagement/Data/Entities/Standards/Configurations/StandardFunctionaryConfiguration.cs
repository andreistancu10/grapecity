using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Standards.Configurations
{
    public class StandardFunctionaryConfiguration : IEntityTypeConfiguration<StandardFunctionary>
    {
        public void Configure(EntityTypeBuilder<StandardFunctionary> builder)
        {
            builder.ToTable(nameof(StandardFunctionary), DocumentManagementDbContext.Schema);

            builder.HasKey(p => p.Id);

            builder.HasOne(item => item.Standard)
                 .WithMany(item => item.StandardFunctionaries)
                 .HasForeignKey(item => item.StandardId);
        }
    }
}
