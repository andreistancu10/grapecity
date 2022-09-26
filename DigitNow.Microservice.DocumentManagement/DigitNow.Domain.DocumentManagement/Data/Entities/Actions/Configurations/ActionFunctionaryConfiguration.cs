using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Actions.Configurations
{
    public class ActionFunctionaryConfiguration : IEntityTypeConfiguration<ActionFunctionary>
    {
        public void Configure(EntityTypeBuilder<ActionFunctionary> builder)
        {
            builder.ToTable(nameof(ActionFunctionary), DocumentManagementDbContext.Schema);

            builder.HasKey(p => p.Id);

            builder.HasOne(item => item.Action)
                 .WithMany(item => item.ActionFunctionaries)
                 .HasForeignKey(item => item.ActionId);
        }
    }
}
