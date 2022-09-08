using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Configurations
{
    public class SpecificObjectiveFunctionaryConfiguration : IEntityTypeConfiguration<SpecificObjectiveFunctionary>
    {
        public void Configure(EntityTypeBuilder<SpecificObjectiveFunctionary> builder)
        {
            builder.ToTable(nameof(SpecificObjectiveFunctionary), DocumentManagementDbContext.Schema);

            builder.HasKey(p => p.Id);

            builder.HasOne(item => item.SpecificObjective)
                 .WithMany(item => item.SpecificObjectiveFunctionarys)
                 .HasPrincipalKey(x => x.ObjectiveId)
                 .HasForeignKey(item => item.SpecificObjectiveId);
        }
    }
}
