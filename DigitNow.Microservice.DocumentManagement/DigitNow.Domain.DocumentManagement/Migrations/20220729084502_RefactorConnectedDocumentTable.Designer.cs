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
    [Migration("20220729084502_RefactorConnectedDocumentTable")]
    partial class RefactorConnectedDocumentTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("DocumentMangement")
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.ConnectedDocument", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("DocumentId")
                        .HasColumnType("bigint");

                    b.Property<long?>("IncomingDocumentId")
                        .HasColumnType("bigint");

                    b.Property<long?>("OutgoingDocumentId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("DocumentId");

                    b.HasIndex("IncomingDocumentId");

                    b.HasIndex("OutgoingDocumentId");

                    b.ToTable("ConnectedDocument", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.ContactDetail", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id")
                        .HasColumnOrder(1);

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
                        .HasColumnType("datetime2")
                        .HasColumnName("CreatedAt")
                        .HasColumnOrder(2);

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatedBy")
                        .HasColumnOrder(3);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Entrance")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Floor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("ModifiedAt")
                        .HasColumnOrder(4);

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("ModifiedBy")
                        .HasColumnOrder(5);

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

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.DeliveryDetail", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreatedAt")
                        .HasColumnOrder(2);

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatedBy")
                        .HasColumnOrder(3);

                    b.Property<int>("DeliveryMode")
                        .HasColumnType("int");

                    b.Property<int>("DirectShipping")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("ModifiedAt")
                        .HasColumnOrder(4);

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("ModifiedBy")
                        .HasColumnOrder(5);

                    b.Property<int>("Post")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("DeliveryDetails", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.Document", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreatedAt")
                        .HasColumnOrder(2);

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatedBy")
                        .HasColumnOrder(3);

                    b.Property<long>("DestinationDepartmentId")
                        .HasColumnType("bigint");

                    b.Property<int>("DocumentType")
                        .HasColumnType("int");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("ModifiedAt")
                        .HasColumnOrder(4);

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("ModifiedBy")
                        .HasColumnOrder(5);

                    b.Property<long?>("RecipientId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("RegistrationNumber")
                        .HasColumnType("bigint");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("StatusModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("StatusModifiedBy")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Document", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.DocumentResolution", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreatedAt")
                        .HasColumnOrder(2);

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatedBy")
                        .HasColumnOrder(3);

                    b.Property<long>("DocumentId")
                        .HasColumnType("bigint");

                    b.Property<int>("DocumentType")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("ModifiedAt")
                        .HasColumnOrder(4);

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("ModifiedBy")
                        .HasColumnOrder(5);

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ResolutionType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("DocumentResolution", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.DocumentUploadedFile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreatedAt")
                        .HasColumnOrder(2);

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatedBy")
                        .HasColumnOrder(3);

                    b.Property<long>("DocumentId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("ModifiedAt")
                        .HasColumnOrder(4);

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("ModifiedBy")
                        .HasColumnOrder(5);

                    b.Property<long>("UploadedFileId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("DocumentId");

                    b.HasIndex("UploadedFileId");

                    b.ToTable("DocumentUploadedFile", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.IncomingDocument", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("ContactDetailId")
                        .HasColumnType("bigint");

                    b.Property<string>("ContentSummary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreatedAt")
                        .HasColumnOrder(2);

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatedBy")
                        .HasColumnOrder(3);

                    b.Property<long?>("DeliveryDetailsId")
                        .HasColumnType("bigint");

                    b.Property<string>("Detail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("DocumentId")
                        .HasColumnType("bigint")
                        .HasColumnName("DocumentId")
                        .HasColumnOrder(6);

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
                        .HasColumnType("datetime2")
                        .HasColumnName("ModifiedAt")
                        .HasColumnOrder(4);

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("ModifiedBy")
                        .HasColumnOrder(5);

                    b.Property<int>("NumberOfPages")
                        .HasColumnType("int");

                    b.Property<double>("ResolutionPeriod")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ContactDetailId");

                    b.HasIndex("DeliveryDetailsId");

                    b.HasIndex("DocumentId")
                        .IsUnique();

                    b.ToTable("IncomingDocument", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.InternalDocument", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreatedAt")
                        .HasColumnOrder(2);

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatedBy")
                        .HasColumnOrder(3);

                    b.Property<int>("DeadlineDaysNumber")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("DocumentId")
                        .HasColumnType("bigint")
                        .HasColumnName("DocumentId")
                        .HasColumnOrder(6);

                    b.Property<int>("InternalDocumentTypeId")
                        .HasColumnType("int");

                    b.Property<bool?>("IsUrgent")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("ModifiedAt")
                        .HasColumnOrder(4);

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("ModifiedBy")
                        .HasColumnOrder(5);

                    b.Property<string>("Observation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SourceDepartmentId")
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
                        .HasColumnType("bigint")
                        .HasColumnName("Id")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("ContactDetailId")
                        .HasColumnType("bigint");

                    b.Property<string>("ContentSummary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreatedAt")
                        .HasColumnOrder(2);

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatedBy")
                        .HasColumnOrder(3);

                    b.Property<long?>("DeliveryDetailsId")
                        .HasColumnType("bigint");

                    b.Property<long>("DocumentId")
                        .HasColumnType("bigint")
                        .HasColumnName("DocumentId")
                        .HasColumnOrder(6);

                    b.Property<string>("DocumentTypeDetail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DocumentTypeId")
                        .HasColumnType("int");

                    b.Property<string>("IdentificationNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("ModifiedAt")
                        .HasColumnOrder(4);

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("ModifiedBy")
                        .HasColumnOrder(5);

                    b.Property<int>("NumberOfPages")
                        .HasColumnType("int");

                    b.Property<string>("RecipientName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContactDetailId");

                    b.HasIndex("DeliveryDetailsId");

                    b.HasIndex("DocumentId")
                        .IsUnique();

                    b.ToTable("OutgoingDocument", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegister", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreatedAt")
                        .HasColumnOrder(2);

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatedBy")
                        .HasColumnOrder(3);

                    b.Property<int>("DocumentCategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("ModifiedAt")
                        .HasColumnOrder(4);

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("ModifiedBy")
                        .HasColumnOrder(5);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Observations")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DocumentCategoryId")
                        .IsUnique();

                    b.ToTable("SpecialRegister", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisterMapping", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreatedAt")
                        .HasColumnOrder(2);

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatedBy")
                        .HasColumnOrder(3);

                    b.Property<long>("DocumentId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("ModifiedAt")
                        .HasColumnOrder(4);

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("ModifiedBy")
                        .HasColumnOrder(5);

                    b.Property<long>("SpecialRegisterId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("DocumentId");

                    b.HasIndex("SpecialRegisterId");

                    b.ToTable("SpecialRegisterMappings", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.UploadedFile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("AbsolutePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreatedAt")
                        .HasColumnOrder(2);

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatedBy")
                        .HasColumnOrder(3);

                    b.Property<long>("DocumentCategoryId")
                        .HasColumnType("bigint");

                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("ModifiedAt")
                        .HasColumnOrder(4);

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("ModifiedBy")
                        .HasColumnOrder(5);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelativePath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UploadedFile", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.WorkflowHistoryLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreatedAt")
                        .HasColumnOrder(2);

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatedBy")
                        .HasColumnOrder(3);

                    b.Property<string>("DeclineReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DestinationDepartmentId")
                        .HasColumnType("int");

                    b.Property<long>("DocumentId")
                        .HasColumnType("bigint");

                    b.Property<int>("DocumentStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("ModifiedAt")
                        .HasColumnOrder(4);

                    b.Property<long>("ModifiedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("ModifiedBy")
                        .HasColumnOrder(5);

                    b.Property<DateTime?>("OpinionRequestedUntil")
                        .HasColumnType("datetime2");

                    b.Property<long>("RecipientId")
                        .HasColumnType("bigint");

                    b.Property<string>("RecipientName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RecipientType")
                        .HasColumnType("int");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Resolution")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DocumentId");

                    b.ToTable("WorkflowHistoryLog", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.ConnectedDocument", b =>
                {
                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.Document", "Document")
                        .WithMany()
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.IncomingDocument", null)
                        .WithMany("ConnectedDocuments")
                        .HasForeignKey("IncomingDocumentId");

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.OutgoingDocument", null)
                        .WithMany("ConnectedDocuments")
                        .HasForeignKey("OutgoingDocumentId");

                    b.Navigation("Document");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.DocumentUploadedFile", b =>
                {
                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.Document", "Document")
                        .WithMany("DocumentUploadedFiles")
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.UploadedFile", "UploadedFile")
                        .WithMany()
                        .HasForeignKey("UploadedFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Document");

                    b.Navigation("UploadedFile");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.IncomingDocument", b =>
                {
                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.ContactDetail", "ContactDetail")
                        .WithMany()
                        .HasForeignKey("ContactDetailId");

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.DeliveryDetail", "DeliveryDetails")
                        .WithMany()
                        .HasForeignKey("DeliveryDetailsId");

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.Document", "Document")
                        .WithOne("IncomingDocument")
                        .HasForeignKey("DigitNow.Domain.DocumentManagement.Data.Entities.IncomingDocument", "DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContactDetail");

                    b.Navigation("DeliveryDetails");

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

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.DeliveryDetail", "DeliveryDetails")
                        .WithMany()
                        .HasForeignKey("DeliveryDetailsId");

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.Document", "Document")
                        .WithOne("OutgoingDocument")
                        .HasForeignKey("DigitNow.Domain.DocumentManagement.Data.Entities.OutgoingDocument", "DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContactDetail");

                    b.Navigation("DeliveryDetails");

                    b.Navigation("Document");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisterMapping", b =>
                {
                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.Document", "Document")
                        .WithMany("SpecialRegisterMappings")
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegister", "SpecialRegister")
                        .WithMany()
                        .HasForeignKey("SpecialRegisterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Document");

                    b.Navigation("SpecialRegister");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.WorkflowHistoryLog", b =>
                {
                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.Entities.Document", "Document")
                        .WithMany("WorkflowHistories")
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Document");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.Document", b =>
                {
                    b.Navigation("DocumentUploadedFiles");

                    b.Navigation("IncomingDocument");

                    b.Navigation("InternalDocument");

                    b.Navigation("OutgoingDocument");

                    b.Navigation("SpecialRegisterMappings");

                    b.Navigation("WorkflowHistories");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.IncomingDocument", b =>
                {
                    b.Navigation("ConnectedDocuments");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.Entities.OutgoingDocument", b =>
                {
                    b.Navigation("ConnectedDocuments");
                });
#pragma warning restore 612, 618
        }
    }
}
