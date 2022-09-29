using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Configurations
{
    public class RiskControlActionConfiguration : IEntityTypeConfiguration<RiskControlAction>
    {
        public void Configure(EntityTypeBuilder<RiskControlAction> builder)
        {
            builder.ToTable(nameof(RiskControlAction), DocumentManagementDbContext.Schema);

            builder.HasKey(p => p.Id);
            builder.Property(p => p.ControlMeasurement).IsRequired();
            builder.Property(p => p.Deadline).IsRequired();

            builder.HasOne(item => item.Risk)
                 .WithMany(item => item.RiskControlActions)
                 .HasForeignKey(item => item.RiskId);
        }
    }
}
