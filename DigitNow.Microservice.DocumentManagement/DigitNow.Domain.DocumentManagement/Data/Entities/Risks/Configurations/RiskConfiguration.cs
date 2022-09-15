using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Risks.Configurations
{
    public class RiskConfiguration : IEntityTypeConfiguration<Risk>
    {
        public void Configure(EntityTypeBuilder<Risk> builder)
        {
            builder.ToTable(nameof(Risk), DocumentManagementDbContext.Schema);

            builder.Property(p => p.GeneralObjectiveId).IsRequired();
            builder.Property(p => p.SpecificObjectiveId).IsRequired();
            builder.Property(p => p.ActivityId).IsRequired();
            builder.Property(p => p.DepartmentId).IsRequired();
            builder.Property(p => p.Code).IsRequired();
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.State).IsRequired();
            builder.Property(p => p.RiskCauses).IsRequired();
            builder.Property(p => p.RiskConsequences).IsRequired();
            builder.Property(p => p.ProbabilityOfApparitionEstimation).IsRequired();
            builder.Property(p => p.ImpactOfObjectivesEstimation).IsRequired();
            builder.Property(p => p.HeadOfDepartmentDecision).IsRequired();
            builder.Property(p => p.AdoptedStrategy).IsRequired();

            builder.HasOne(item => item.AssociatedGeneralObjective)
                .WithMany(item => item.Risks)
                .HasPrincipalKey(x => x.ObjectiveId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(item => item.GeneralObjectiveId);

            builder.HasOne(item => item.AssociatedSpecificObjective)
                .WithMany(item => item.Risks)
                .HasPrincipalKey(x => x.ObjectiveId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(item => item.SpecificObjectiveId);

            builder.HasOne(item => item.AssociatedActivity)
                .WithMany(item => item.Risks)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(item => item.ActivityId);

            builder.HasOne(item => item.AssociatedAction)
                .WithMany(item => item.Risks)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(item => item.ActionId);
        }
    }
}
