using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Data.Entities.Objectives.Configurations
{
    public class SpecificObjectiveFunctionaryConfiguration : IEntityTypeConfiguration<SpecificObjectiveFunctionary>
    {
        public void Configure(EntityTypeBuilder<SpecificObjectiveFunctionary> builder)
        {
            builder.ToTable(nameof(SpecificObjectiveFunctionary), DocumentManagementDbContext.Schema);

            builder.HasKey(p => p.Id);

            builder.HasOne(item => item.SpecificObjective)
                 .WithMany(item => item.SpecificObjectiveFunctionarys)
                 .HasForeignKey(item => item.SpecificObjectiveId);
        }
    }
}
