using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Configuration
{
    public class ProcedureConfiguration : IEntityTypeConfiguration<Procedure>
    {
        public void Configure(EntityTypeBuilder<Procedure> builder)
        {
            builder.ToTable(nameof(Procedure), DocumentManagementDbContext.Schema);

            builder.Property(p => p.GeneralObjectiveId).IsRequired();
            builder.Property(p => p.SpecificObjectiveId).IsRequired();
            builder.Property(p => p.ActivityId).IsRequired();
            builder.Property(p => p.DepartmentId).IsRequired();
            builder.Property(p => p.Code).IsRequired();
            builder.Property(p => p.Title).IsRequired();
            builder.Property(p => p.State).IsRequired();
            builder.Property(p => p.Edition).IsRequired();
            builder.Property(p => p.StartDate).IsRequired();
            builder.Property(p => p.Scope).IsRequired();
            builder.Property(p => p.DomainOfApplicability).IsRequired();
            builder.Property(p => p.ProcedureDescription).IsRequired();
            builder.Property(p => p.Responsibility).IsRequired();

            builder.HasOne(item => item.AssociatedGeneralObjective)
                .WithMany(item => item.Procedures)
                .HasPrincipalKey(x => x.ObjectiveId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(item => item.GeneralObjectiveId);

            builder.HasOne(item => item.AssociatedSpecificObjective)
                .WithMany(item => item.Procedures)
                .HasPrincipalKey(x => x.ObjectiveId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(item => item.SpecificObjectiveId);

            builder.HasOne(item => item.AssociatedActivity)
                .WithMany(item => item.Procedures)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(item => item.ActivityId);
        }
    }
}