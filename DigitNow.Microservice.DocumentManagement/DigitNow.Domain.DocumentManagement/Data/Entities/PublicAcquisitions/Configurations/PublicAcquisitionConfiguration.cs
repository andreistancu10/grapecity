using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.PublicAcquisitions.Configurations
{
    public class PublicAcquisitionConfiguration : IEntityTypeConfiguration<PublicAcquisitionProject>
    {
        public void Configure(EntityTypeBuilder<PublicAcquisitionProject> builder)
        {
            builder.ToTable(nameof(PublicAcquisitionProject), DocumentManagementDbContext.Schema);

            builder.Property(p => p.ProjectYear).IsRequired();
            builder.Property(p => p.Type).IsRequired();
            builder.Property(p => p.CpvCode).IsRequired();
            builder.Property(p => p.EstimatedValue).IsRequired();
            builder.Property(p => p.FinancingSource).IsRequired();
            builder.Property(p => p.EstablishedProcedure).IsRequired();
            builder.Property(p => p.EstimatedMonthForInitiatingProcedure).IsRequired();
            builder.Property(p => p.EstimatedMonthForProcedureAssignment).IsRequired();
            builder.Property(p => p.ProcedureAssignmentMethod).IsRequired();
            builder.Property(p => p.ProcedureAssignmentResponsible).IsRequired();
        }
    }
}
