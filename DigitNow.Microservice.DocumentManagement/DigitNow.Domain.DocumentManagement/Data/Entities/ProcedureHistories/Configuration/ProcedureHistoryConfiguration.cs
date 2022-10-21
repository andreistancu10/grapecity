using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Configuration
{
    public class ProcedureHistoryConfiguration : IEntityTypeConfiguration<ProcedureHistory>
    {
        public void Configure(EntityTypeBuilder<ProcedureHistory> builder)
        {
            builder.ToTable(nameof(Procedure), DocumentManagementDbContext.Schema);
        }
    }
}