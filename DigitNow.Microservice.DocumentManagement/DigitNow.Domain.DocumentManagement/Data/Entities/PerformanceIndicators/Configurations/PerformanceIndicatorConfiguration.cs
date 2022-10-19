using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.PerformanceIndicators.Configurations
{
    public class PerformanceIndicatorConfiguration : IEntityTypeConfiguration<PerformanceIndicator>
    {
        public void Configure(EntityTypeBuilder<PerformanceIndicator> builder)
        {
            builder.ToTable(nameof(PerformanceIndicator), DocumentManagementDbContext.Schema);

            builder.Property(p => p.GeneralObjectiveId).IsRequired();
            builder.Property(p => p.SpecificObjectiveId).IsRequired();
            builder.Property(p => p.DepartmentId).IsRequired();
            builder.Property(p => p.Code).IsRequired();
            builder.Property(p => p.Title).IsRequired();
            builder.Property(p => p.StateId).IsRequired();
            builder.Property(p => p.Title).IsRequired();
            builder.Property(p => p.QuantificationFormula).IsRequired();
            builder.Property(p => p.ResultIndicator).IsRequired();

            builder.HasOne(item => item.AssociatedGeneralObjective)
                .WithMany(item => item.PerformanceIndicators)
                .HasPrincipalKey(x => x.ObjectiveId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(item => item.GeneralObjectiveId);

            builder.HasOne(item => item.AssociatedSpecificObjective)
                .WithMany(item => item.PerformanceIndicators)
                .HasPrincipalKey(x => x.ObjectiveId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(item => item.SpecificObjectiveId);

            builder.HasOne(item => item.AssociatedActivity)
                .WithMany(item => item.PerformanceIndicators)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(item => item.ActivityId);
        }
    }
}
