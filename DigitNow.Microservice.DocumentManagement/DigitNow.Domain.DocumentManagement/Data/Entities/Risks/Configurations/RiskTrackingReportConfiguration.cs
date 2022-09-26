using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Risks.Configurations
{
    public class RiskTrackingReportConfiguration : IEntityTypeConfiguration<RiskTrackingReport>
    {
        public void Configure(EntityTypeBuilder<RiskTrackingReport> builder)
        {
            builder.ToTable(nameof(RiskTrackingReport), DocumentManagementDbContext.Schema);

            builder.Property(p => p.ControlMeasuresImplementationState).IsRequired();
            builder.Property(p => p.ProbabilityOfApparitionEstimation).IsRequired();
            builder.Property(p => p.ImpactOfObjectivesEstimation).IsRequired();
            builder.Property(p => p.RiskExposureEvaluation).IsRequired();
        }
    }
}
