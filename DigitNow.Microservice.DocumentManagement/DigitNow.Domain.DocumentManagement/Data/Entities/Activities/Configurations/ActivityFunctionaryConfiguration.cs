using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Activities.Configurations
{
    public class ActivityFunctionaryConfiguration : IEntityTypeConfiguration<ActivityFunctionary>
    {
        public void Configure(EntityTypeBuilder<ActivityFunctionary> builder)
        {
            builder.ToTable(nameof(ActivityFunctionary), DocumentManagementDbContext.Schema);

            builder.HasKey(p => p.Id);

            builder.HasOne(item => item.Activity)
                 .WithMany(item => item.ActivityFunctionarys)
                 .HasForeignKey(item => item.ActivityId);
        }
    }
}
