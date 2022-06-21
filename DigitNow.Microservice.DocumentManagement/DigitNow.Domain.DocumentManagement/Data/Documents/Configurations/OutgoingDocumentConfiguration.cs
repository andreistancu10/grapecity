﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitNow.Domain.DocumentManagement.Data.Documents.Configurations;

public class OutgoingDocumentConfiguration : IEntityTypeConfiguration<OutgoingDocument>
{
    public void Configure(EntityTypeBuilder<OutgoingDocument> builder)
    {
        builder.ToTable(nameof(OutgoingDocument), DocumentManagementDbContext.Schema);

        builder.Property(p => p.RecipientTypeId).IsRequired();
        builder.Property(p => p.RecipientName).IsRequired();
        builder.Property(p => p.IdentificationNumber).IsRequired();
        builder.Property(p => p.ContactDetailId).IsRequired();
        builder.Property(p => p.ContentSummary).IsRequired();
        builder.Property(p => p.NumberOfPages).IsRequired();
        builder.Property(p => p.RecipientId).IsRequired();
        builder.Property(p => p.DocumentTypeId).IsRequired();
        builder.Property(p => p.DocumentTypeDetail).IsRequired();
        builder.Property(p => p.CreationDate).IsRequired();

        builder.HasOne(item => item.Document)
            .WithOne(item => item.OutgoingDocument)
            .HasForeignKey<OutgoingDocument>(item => item.DocumentId);

        builder.HasMany(item => item.ConnectedDocuments)
            .WithOne(item => item.OutgoingDocument)
            .HasForeignKey(item => item.Id);
    }
}