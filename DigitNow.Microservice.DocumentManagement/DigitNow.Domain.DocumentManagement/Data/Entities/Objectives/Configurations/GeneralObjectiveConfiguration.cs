using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Objectives.Configurations
{
    public class GeneralObjectiveConfiguration : IEntityTypeConfiguration<GeneralObjective>
    {
        public void Configure(EntityTypeBuilder<GeneralObjective> builder)
        {
            builder.ToTable(nameof(GeneralObjective), DocumentManagementDbContext.Schema);

            builder.HasOne(item => item.Objective)
                 .WithOne(item => item.GeneralObjective)
                 .HasForeignKey<GeneralObjective>(item => item.ObjectiveId);
        }
    }
}