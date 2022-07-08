﻿// <auto-generated />
using System;
using DigitNow.Domain.DocumentManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DigitNow.Domain.DocumentManagement.Migrations
{
    [DbContext(typeof(DocumentManagementDbContext))]
    [Migration("20220629133814_AssociateDocumentToSpecialRegister")]
    partial class AssociateDocumentToSpecialRegister
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("DocumentMangement")
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.DocumentSpecialRegisters.DocumentSpecialRegister", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("AssociationId")
                        .HasColumnType("bigint");

                    b.Property<int>("DocumentId")
                        .HasColumnType("int");

                    b.Property<long?>("DocumentId1")
                        .HasColumnType("bigint");

                    b.Property<long>("SpecialRegisterId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("DocumentId1");

                    b.HasIndex("Id");

                    b.HasIndex("SpecialRegisterId");

                    b.ToTable("DocumentSpecialRegisterAssociations", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.ConnectedDocument", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("ChildDocumentId")
                        .HasColumnType("bigint");

                    b.Property<int>("DocumentType")
                        .HasColumnType("int");

                    b.Property<long?>("IncomingDocumentId")
                        .HasColumnType("bigint");

                    b.Property<long?>("OutgoingDocumentId")
                        .HasColumnType("bigint");

                    b.Property<long>("RegistrationNumber")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("IncomingDocumentId");

                    b.HasIndex("OutgoingDocumentId");

                    b.ToTable("ConnectedDocument", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.ContactDetail", b =>
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

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Entrance")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Floor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("bigint");

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

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.Document", b =>
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

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.DocumentResolution", b =>
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

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.IncomingDocument", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("ContactDetailId")
                        .HasColumnType("bigint");

                    b.Property<string>("ContentSummary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint");

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

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<int>("NumberOfPages")
                        .HasColumnType("int");

                    b.Property<int>("RecipientId")
                        .HasColumnType("int");

                    b.Property<double>("ResolutionPeriod")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ContactDetailId");

                    b.HasIndex("DocumentId")
                        .IsUnique();

                    b.ToTable("IncomingDocument", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.InternalDocument", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint");

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

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<string>("Observation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReceiverDepartmentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DocumentId")
                        .IsUnique();

                    b.ToTable("InternalDocument", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.OutgoingDocument", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("ContactDetailId")
                        .HasColumnType("bigint");

                    b.Property<string>("ContentSummary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<long>("DocumentId")
                        .HasColumnType("bigint");

                    b.Property<string>("DocumentTypeDetail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DocumentTypeId")
                        .HasColumnType("int");

                    b.Property<string>("IdentificationNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("ModifiedBy")
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

                    b.HasKey("Id");

                    b.HasIndex("ContactDetailId");

                    b.HasIndex("DocumentId")
                        .IsUnique();

                    b.ToTable("OutgoingDocument", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.RegistrationNumberCounter", b =>
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

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisters.SpecialRegister", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<int>("DocumentCategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Observations")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DocumentCategoryId")
                        .IsUnique();

                    b.ToTable("SpecialRegister", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.WorkflowHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeclineReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("IncomingDocumentId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("OpinionRequestedUntil")
                        .HasColumnType("datetime2");

                    b.Property<long?>("OutgoingDocumentId")
                        .HasColumnType("bigint");

                    b.Property<long>("RecipientId")
                        .HasColumnType("bigint");

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

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.DocumentSpecialRegisters.DocumentSpecialRegister", b =>
                {
                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.IncomingDocument", "Document")
                        .WithMany()
                        .HasForeignKey("DocumentId1");

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisters.SpecialRegister", "SpecialRegister")
                        .WithMany()
                        .HasForeignKey("SpecialRegisterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Document");

                    b.Navigation("SpecialRegister");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.ConnectedDocument", b =>
                {
                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.IncomingDocument", null)
                        .WithMany("ConnectedDocuments")
                        .HasForeignKey("IncomingDocumentId");

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.OutgoingDocument", null)
                        .WithMany("ConnectedDocuments")
                        .HasForeignKey("OutgoingDocumentId");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.IncomingDocument", b =>
                {
                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.ContactDetail", "ContactDetail")
                        .WithMany()
                        .HasForeignKey("ContactDetailId");

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.Document", "Document")
                        .WithOne("IncomingDocument")
                        .HasForeignKey("DigitNow.Domain.DocumentManagement.Data.Entities.IncomingDocument", "DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContactDetail");

                    b.Navigation("Document");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.InternalDocument", b =>
                {
                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.Document", "Document")
                        .WithOne("InternalDocument")
                        .HasForeignKey("DigitNow.Domain.DocumentManagement.Data.Entities.InternalDocument", "DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Document");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.OutgoingDocument", b =>
                {
                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.ContactDetail", "ContactDetail")
                        .WithMany()
                        .HasForeignKey("ContactDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.Document", "Document")
                        .WithOne("OutgoingDocument")
                        .HasForeignKey("DigitNow.Domain.DocumentManagement.Data.Entities.OutgoingDocument", "DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContactDetail");

                    b.Navigation("Document");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.WorkflowHistory", b =>
                {
                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.IncomingDocument", null)
                        .WithMany("WorkflowHistory")
                        .HasForeignKey("IncomingDocumentId");

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.OutgoingDocument", null)
                        .WithMany("WorkflowHistory")
                        .HasForeignKey("OutgoingDocumentId");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.Document", b =>
                {
                    b.Navigation("IncomingDocument");

                    b.Navigation("InternalDocument");

                    b.Navigation("OutgoingDocument");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.IncomingDocument", b =>
                {
                    b.Navigation("ConnectedDocuments");

                    b.Navigation("WorkflowHistory");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.OutgoingDocument", b =>
                {
                    b.Navigation("ConnectedDocuments");

                    b.Navigation("WorkflowHistory");
                });
#pragma warning restore 612, 618
        }
    }
}
