using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Objectives.Configurations
{
    public class SpecificObjectiveConfiguration : IEntityTypeConfiguration<SpecificObjective>
    {
        public void Configure(EntityTypeBuilder<SpecificObjective> builder)
        {
            builder.ToTable(nameof(SpecificObjective), DocumentManagementDbContext.Schema);

            builder.Property(p => p.DepartmentId).IsRequired();
            builder.Property(p => p.GeneralObjectiveId).IsRequired();

            builder.HasOne(item => item.Objective)
                .WithOne(item => item.SpecificObjective)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey<SpecificObjective>(item => item.ObjectiveId);


            builder.HasOne(item => item.AssociatedGeneralObjective)
                .WithMany(item => item.SpecificObjectives)
                .HasForeignKey(item => item.GeneralObjectiveId);

        }
    }
}
