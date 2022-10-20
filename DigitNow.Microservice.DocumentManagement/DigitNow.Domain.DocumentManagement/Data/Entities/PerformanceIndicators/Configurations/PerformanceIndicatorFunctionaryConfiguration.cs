using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.PerformanceIndicators.Configurations
{
    public class PerformanceIndicatorFunctionaryConfiguration : IEntityTypeConfiguration<PerformanceIndicatorFunctionary>
    {
        public void Configure(EntityTypeBuilder<PerformanceIndicatorFunctionary> builder)
        {
            builder.ToTable(nameof(PerformanceIndicatorFunctionary), DocumentManagementDbContext.Schema);

            builder.HasKey(p => p.Id);

            builder.HasOne(item => item.PerformanceIndicator)
                 .WithMany(item => item.PerformanceIndicatorFunctionaries)
                 .HasForeignKey(item => item.PerformanceIndicatorId);
        }
    }
}
