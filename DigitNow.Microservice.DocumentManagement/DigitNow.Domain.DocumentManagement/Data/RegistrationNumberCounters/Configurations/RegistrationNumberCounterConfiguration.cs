using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.RegistrationNumberCounters.Configurations;

public class RegistrationNumberCounterConfiguration
{
    public void Configure(EntityTypeBuilder<RegistrationNumberCounter> builder)
    {
        builder.ToTable(nameof(RegistrationNumberCounter), DocumentManagementDbContext.Schema);

        builder.HasKey(p => p.Id);

        builder.HasData(Seed.Data.GetRegistrationNumberInitialValue());
    }
}