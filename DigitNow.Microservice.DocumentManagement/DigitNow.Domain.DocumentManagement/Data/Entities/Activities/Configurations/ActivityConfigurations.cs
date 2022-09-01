using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Activities.Configurations
{
    public class ActivityConfigurations : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.ToTable(nameof(Activity), DocumentManagementDbContext.Schema);

            builder.Property(p => p.DepartmentId).IsRequired();
            builder.Property(p => p.GeneralObjectiveId).IsRequired();

            builder.HasOne(item => item.AssociatedGeneralObjective)
                .WithMany(item => item.Activities)
                .HasPrincipalKey(x => x.ObjectiveId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(item => item.GeneralObjectiveId);

            builder.HasOne(item => item.AssociatedSpecificObjective)
                .WithMany(item => item.Activities)
                .HasPrincipalKey(x => x.ObjectiveId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(item => item.SpecificObjectiveId);
        }
    }
}
