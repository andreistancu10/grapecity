using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.ContactDetails.Configurations;

public class ContactDetailConfiguration : IEntityTypeConfiguration<ContactDetail>
{
    public void Configure(EntityTypeBuilder<ContactDetail> builder)
    {
        builder.ToTable(nameof(ContactDetail), DocumentManagementDbContext.Schema);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.CityId).IsRequired();
        builder.Property(p => p.CountryId).IsRequired();
        builder.Property(p => p.CountyId).IsRequired();
    }
}