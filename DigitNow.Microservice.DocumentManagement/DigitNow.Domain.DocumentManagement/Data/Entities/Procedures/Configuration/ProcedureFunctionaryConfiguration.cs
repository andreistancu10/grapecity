using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Configuration
{
    public class ProcedureFunctionaryConfiguration : IEntityTypeConfiguration<ProcedureFunctionary>
    {
        public void Configure(EntityTypeBuilder<ProcedureFunctionary> builder)
        {
            builder.ToTable(nameof(ProcedureFunctionary), DocumentManagementDbContext.Schema);

            builder.HasKey(p => p.Id);

            builder.HasOne(item => item.Procedure)
                 .WithMany(item => item.ProcedureFunctionaries)
                 .HasForeignKey(item => item.ProcedureId);
        }
    }
}
