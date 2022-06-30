using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Entities;

public class RegistrationNumberCounterConfiguration
{
    public void Configure(EntityTypeBuilder<RegistrationNumberCounter> builder)
    {
        builder.ToTable(nameof(RegistrationNumberCounter), DocumentManagementDbContext.Schema);

        builder.HasKey(p => p.Id);

        builder.HasData(Seeds.RegistrationNumberCounters.Data.GetRegistrationNumberInitialValue());
    }
}