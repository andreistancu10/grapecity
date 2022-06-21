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
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ChildDocumentId")
                        .HasColumnType("int");

                    b.Property<int>("DocumentType")
                        .HasColumnType("int");

                    b.Property<int?>("IncomingDocumentId")
                        .HasColumnType("int");

                    b.Property<long?>("OutgoingDocumentId")
                        .HasColumnType("bigint");

                    b.Property<int>("RegistrationNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IncomingDocumentId");

                    b.HasIndex("OutgoingDocumentId");

                    b.ToTable("ConnectedDocument", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.ContactDetails.ContactDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

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

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.IncomingConnectedDocuments.IncomingConnectedDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ChildIncomingDocumentId")
                        .HasColumnType("int");

                    b.Property<int>("DocumentType")
                        .HasColumnType("int");

                    b.Property<int?>("IncomingDocumentId")
                        .HasColumnType("int");

                    b.Property<int>("RegistrationNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IncomingDocumentId");

                    b.ToTable("IncomingConnectedDocument", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.IncomingDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("ContactDetailId")
                        .HasColumnType("int");

                    b.Property<string>("ContentSummary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Detail")
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<DateTime?>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RegistrationNumber")
                        .HasColumnType("int");

                    b.Property<double>("ResolutionPeriod")
                        .HasColumnType("float");

                    b.Property<string>("User")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContactDetailId");

                    b.ToTable("IncomingDocument", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.InternalDocuments.InternalDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeadlineDaysNumber")
                        .HasColumnType("int");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InternalDocumentTypeId")
                        .HasColumnType("int");

                    b.Property<bool?>("IsUrgent")
                        .HasColumnType("bit");

                    b.Property<string>("Observation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReceiverDepartmentId")
                        .HasColumnType("int");

                    b.Property<int>("RegistrationNumber")
                        .HasColumnType("int");

                    b.Property<string>("User")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("InternalDocument", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.OutgoingConnectedDocuments.OutgoingConnectedDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<long>("ChildOutgoingDocumentId")
                        .HasColumnType("bigint");

                    b.Property<int>("DocumentType")
                        .HasColumnType("int");

                    b.Property<int?>("OutgoingDocumentId")
                        .HasColumnType("int");

                    b.Property<int>("RegistrationNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OutgoingDocumentId");

                    b.ToTable("OutgoingConnectedDocument", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments.OutgoingDocument", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<int?>("ContactDetailId")
                        .HasColumnType("int");

                    b.Property<string>("ContentSummary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DocumentTypeDetail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DocumentTypeId")
                        .HasColumnType("int");

                    b.Property<long?>("IdentificationNumber")
                        .HasColumnType("bigint");

                    b.Property<int>("NumberOfPages")
                        .HasColumnType("int");

                    b.Property<int>("RecipientId")
                        .HasColumnType("int");

                    b.Property<string>("RecipientName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RegistrationNumber")
                        .HasColumnType("int");

                    b.Property<string>("User")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ContactDetailId");

                    b.ToTable("OutgoingDocument", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.RegistrationNumberCounter.RegistrationNumberCounter", b =>
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

                    b.ToTable("RegistrationNumberCounter", "DocumentMangement");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.WorkflowHistories.WorkflowHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("ActionType")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeclineReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IncomingDocumentId")
                        .HasColumnType("int");

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

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.ConnectedDocuments.ConnectedDocument", b =>
                {
                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.IncomingDocument", null)
                        .WithMany("ConnectedDocuments")
                        .HasForeignKey("IncomingDocumentId");

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments.OutgoingDocument", null)
                        .WithMany("ConnectedDocuments")
                        .HasForeignKey("OutgoingDocumentId");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.IncomingDocument", b =>
                {
                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.ContactDetails.ContactDetail", "ContactDetail")
                        .WithMany()
                        .HasForeignKey("ContactDetailId");

                    b.Navigation("ContactDetail");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments.OutgoingDocument", b =>
                {
                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.ContactDetails.ContactDetail", "ContactDetail")
                        .WithMany()
                        .HasForeignKey("ContactDetailId");

                    b.Navigation("ContactDetail");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.WorkflowHistories.WorkflowHistory", b =>
                {
                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.IncomingDocument", null)
                        .WithMany("WorkflowHistory")
                        .HasForeignKey("IncomingDocumentId");

                    b.HasOne("DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments.OutgoingDocument", null)
                        .WithMany("WorkflowHistory")
                        .HasForeignKey("OutgoingDocumentId");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.IncomingDocuments.IncomingDocument", b =>
                {
                    b.Navigation("ConnectedDocuments");

                    b.Navigation("WorkflowHistory");
                });

            modelBuilder.Entity("DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments.OutgoingDocument", b =>
                {
                    b.Navigation("ConnectedDocuments");

                    b.Navigation("WorkflowHistory");
                });
#pragma warning restore 612, 618
        }
    }
}
