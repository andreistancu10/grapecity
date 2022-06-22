﻿// <auto-generated />
using System;
using DigitNow.Domain.DocumentManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    [DbContext(typeof(DocumentManagementDbContext))]
    partial class DocumentManagementDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("DocumentMangement")
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.ConnectedDocuments.ConnectedDocument", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<long>("ChildDocumentId")
                        .HasColumnType("bigint");

                    b.Property<int>("DocumentType")
                        .HasColumnType("int");

                    b.Property<long>("RegistrationNumber")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("ConnectedDocument", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.ContactDetails.ContactDetail", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("ApartmentNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Building")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<int>("CountyId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Entrance")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Floor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ContactDetail", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Documents.Document", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<int>("DocumentType")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RegistrationNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Document", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Documents.DocumentResolution", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<long>("DocumentId")
                        .HasColumnType("bigint");

                    b.Property<int>("DocumentType")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ResolutionType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("DocumentResolution", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.RegistrationNumberCounters.RegistrationNumberCounter", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RegistrationNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("RegistrationNumberCounters", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.WorkflowHistories.WorkflowHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<int?>("ActionType")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeclineReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("IncomingDocumentId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("OpinionRequestedUntil")
                        .HasColumnType("datetime2");

                    b.Property<long?>("OutgoingDocumentId")
                        .HasColumnType("bigint");

                    b.Property<int>("RecipientId")
                        .HasColumnType("int");

                    b.Property<string>("RecipientName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RecipientType")
                        .HasColumnType("int");

                    b.Property<int>("RegistrationNumber")
                        .HasColumnType("int");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Resolution")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IncomingDocumentId");

                    b.HasIndex("OutgoingDocumentId");

                    b.ToTable("WorkflowHistory", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Documents.IncomingDocument", b =>
                {
                    b.HasBaseType("DigitNow.Domain.DocumentManagement.Data.Documents.Document");

                    b.Property<long?>("ContactDetailId")
                        .HasColumnType("bigint");

                    b.Property<string>("ContentSummary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Detail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("DocumentId")
                        .HasColumnType("bigint");

                    b.Property<int>("DocumentTypeId")
                        .HasColumnType("int");

                    b.Property<int>("ExternalNumber")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ExternalNumberDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("IdentificationNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InputChannelId")
                        .HasColumnType("int");

                    b.Property<bool?>("IsGDPRAgreed")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsUrgent")
                        .HasColumnType("bit");

                    b.Property<string>("IssuerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IssuerTypeId")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfPages")
                        .HasColumnType("int");

                    b.Property<int>("RecipientId")
                        .HasColumnType("int");

                    b.Property<double>("ResolutionPeriod")
                        .HasColumnType("float");

                    b.HasIndex("ContactDetailId");

                    b.HasIndex("DocumentId")
                        .IsUnique()
                        .HasFilter("[DocumentId] IS NOT NULL");

                    b.ToTable("IncomingDocument", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Documents.InternalDocument", b =>
                {
                    b.HasBaseType("DigitNow.Domain.DocumentManagement.Data.Documents.Document");

                    b.Property<int>("DeadlineDaysNumber")
                        .HasColumnType("int");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("DocumentId")
                        .HasColumnType("bigint");

                    b.Property<int>("InternalDocumentTypeId")
                        .HasColumnType("int");

                    b.Property<bool?>("IsUrgent")
                        .HasColumnType("bit");

                    b.Property<string>("Observation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReceiverDepartmentId")
                        .HasColumnType("int");

                    b.HasIndex("DocumentId")
                        .IsUnique()
                        .HasFilter("[DocumentId] IS NOT NULL");

                    b.ToTable("InternalDocument", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Documents.OutgoingDocument", b =>
                {
                    b.HasBaseType("DigitNow.Domain.DocumentManagement.Data.Documents.Document");

                    b.Property<int>("ContactDetailId")
                        .HasColumnType("int");

                    b.Property<long?>("ContactDetailId1")
                        .HasColumnType("bigint");

                    b.Property<string>("ContentSummary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("DocumentId")
                        .HasColumnType("bigint");

                    b.Property<string>("DocumentTypeDetail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DocumentTypeId")
                        .HasColumnType("int");

                    b.Property<long?>("IdentificationNumber")
                        .IsRequired()
                        .HasColumnType("bigint");

                    b.Property<int>("NumberOfPages")
                        .HasColumnType("int");

                    b.Property<int>("RecipientId")
                        .HasColumnType("int");

                    b.Property<string>("RecipientName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RecipientTypeId")
                        .HasColumnType("int");

                    b.HasIndex("ContactDetailId1");

                    b.HasIndex("DocumentId")
                        .IsUnique()
                        .HasFilter("[DocumentId] IS NOT NULL");

                    b.ToTable("OutgoingDocument", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.ConnectedDocuments.ConnectedDocument", b =>
                {
                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Documents.IncomingDocument", "IncomingDocument")
                        .WithMany("ConnectedDocuments")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Documents.OutgoingDocument", "OutgoingDocument")
                        .WithMany("ConnectedDocuments")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("IncomingDocument");

                    b.Navigation("OutgoingDocument");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.WorkflowHistories.WorkflowHistory", b =>
                {
                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Documents.IncomingDocument", null)
                        .WithMany("WorkflowHistory")
                        .HasForeignKey("IncomingDocumentId");

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Documents.OutgoingDocument", null)
                        .WithMany("WorkflowHistory")
                        .HasForeignKey("OutgoingDocumentId");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Documents.IncomingDocument", b =>
                {
                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.ContactDetails.ContactDetail", "ContactDetail")
                        .WithMany()
                        .HasForeignKey("ContactDetailId");

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Documents.Document", "Document")
                        .WithOne("IncomingDocument")
                        .HasForeignKey("DigitNow.Domain.DocumentManagement.Data.Documents.IncomingDocument", "DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Documents.Document", null)
                        .WithOne()
                        .HasForeignKey("DigitNow.Domain.DocumentManagement.Data.Documents.IncomingDocument", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("ContactDetail");

                    b.Navigation("Document");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Documents.InternalDocument", b =>
                {
                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Documents.Document", "Document")
                        .WithOne("InternalDocument")
                        .HasForeignKey("DigitNow.Domain.DocumentManagement.Data.Documents.InternalDocument", "DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Documents.Document", null)
                        .WithOne()
                        .HasForeignKey("DigitNow.Domain.DocumentManagement.Data.Documents.InternalDocument", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Document");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Documents.OutgoingDocument", b =>
                {
                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.ContactDetails.ContactDetail", "ContactDetail")
                        .WithMany()
                        .HasForeignKey("ContactDetailId1");

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Documents.Document", "Document")
                        .WithOne("OutgoingDocument")
                        .HasForeignKey("DigitNow.Domain.DocumentManagement.Data.Documents.OutgoingDocument", "DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Documents.Document", null)
                        .WithOne()
                        .HasForeignKey("DigitNow.Domain.DocumentManagement.Data.Documents.OutgoingDocument", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("ContactDetail");

                    b.Navigation("Document");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Documents.Document", b =>
                {
                    b.Navigation("IncomingDocument");

                    b.Navigation("InternalDocument");

                    b.Navigation("OutgoingDocument");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Documents.IncomingDocument", b =>
                {
                    b.Navigation("ConnectedDocuments");

                    b.Navigation("WorkflowHistory");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Documents.OutgoingDocument", b =>
                {
                    b.Navigation("ConnectedDocuments");

                    b.Navigation("WorkflowHistory");
                });
#pragma warning restore 612, 618
        }
    }
}
