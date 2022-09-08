using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Actions.Configurations
{
    public class ActionConfiguration : IEntityTypeConfiguration<Action>
    {
        public void Configure(EntityTypeBuilder<Action> builder)
        {
            builder.ToTable(nameof(Action), DocumentManagementDbContext.Schema);

            builder.Property(p => p.DepartmentId).IsRequired();
            builder.Property(p => p.ActivityId).IsRequired();

            builder.HasOne(item => item.AssociatedActivity)
                .WithMany(item => item.Actions)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(item => item.ActivityId);
        }
    }
}
