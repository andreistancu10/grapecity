using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Objectives.Configurations
{
    public class ObjectiveConfiguration : IEntityTypeConfiguration<Objective>
    {
        public void Configure(EntityTypeBuilder<Objective> builder)
        {
            builder.ToTable(nameof(Objective), DocumentManagementDbContext.Schema);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.ObjectiveType).IsRequired();
            builder.Property(p => p.Code).IsRequired();
            builder.Property(p => p.State).IsRequired();
            builder.Property(p => p.Title).IsRequired();
            builder.Property(p => p.Details).IsRequired();
        }
    }
}
