using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Configurations
{
    public class RiskActionProposalConfiguration : IEntityTypeConfiguration<RiskActionProposal>
    {
        public void Configure(EntityTypeBuilder<RiskActionProposal> builder)
        {
            builder.ToTable(nameof(RiskActionProposal), DocumentManagementDbContext.Schema);

            builder.Property(p => p.ProposedAction).IsRequired();
            builder.Property(p => p.RiskTrackingReportDate).IsRequired();
            builder.Property(p => p.Deadline).IsRequired();
        }
    }
}
