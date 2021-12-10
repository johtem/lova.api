﻿// <auto-generated />
using System;
using LOVA.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LOVA.API.Migrations
{
    [DbContext(typeof(LovaDbContext))]
    [Migration("20211210120123_AddedDescriptonToMailType")]
    partial class AddedDescriptonToMailType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LOVA.API.Models.Activity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Address")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<int>("Master_node")
                        .HasColumnType("int");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("LOVA.API.Models.ActivityCount", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AverageCount")
                        .HasColumnType("int");

                    b.Property<int>("CountActivity")
                        .HasColumnType("int");

                    b.Property<DateTime>("Hourly")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("ActivityCounts");
                });

            modelBuilder.Entity("LOVA.API.Models.ActivityPerRow", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsGroupAddress")
                        .HasColumnType("bit");

                    b.Property<double>("TimeDiff")
                        .HasColumnType("float");

                    b.Property<DateTime>("TimeDown")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TimeUp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("ActivityPerRows");
                });

            modelBuilder.Entity("LOVA.API.Models.DrainPatrolAlarm", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AlarmType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<int>("Limit")
                        .HasColumnType("int");

                    b.Property<int>("Master_node")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("DrainPatrolAlarms");
                });

            modelBuilder.Entity("LOVA.API.Models.FullDrain", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateFulldrain")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsUpdated")
                        .HasColumnType("bit");

                    b.Property<DateTime>("MailSent")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("FullDrains");
                });

            modelBuilder.Entity("LOVA.API.Models.IssueReport", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Alarm")
                        .HasColumnType("int");

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("AspNetUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsChargeable")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLowVacuum")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPhoto")
                        .HasColumnType("bit");

                    b.Property<int>("MasterNode")
                        .HasColumnType("int");

                    b.Property<string>("NewActivatorSerialNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewValveSerialNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldActivatorSerialNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldValveSerialNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProblemDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SolutionDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TimeForAlarm")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TimeToRepair")
                        .HasColumnType("decimal(5,2)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("WellId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("WellId");

                    b.ToTable("IssueReports");
                });

            modelBuilder.Entity("LOVA.API.Models.Lova.LatestMaintenance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("LastMaintenance")
                        .HasColumnType("datetime2");

                    b.Property<int>("MaintenanceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("NextMaintenance")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("MaintenanceId");

                    b.ToTable("LatestMaintenances");
                });

            modelBuilder.Entity("LOVA.API.Models.Lova.Maintenance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MaintenanceGroup")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RecurringFrequence")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Maintenances");
                });

            modelBuilder.Entity("LOVA.API.Models.LovaIssue", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Action")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("OwnedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Response")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("LovaIssues");
                });

            modelBuilder.Entity("LOVA.API.Models.MailSubscription", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsScription")
                        .HasColumnType("bit");

                    b.Property<long>("MailTypeId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MailTypeId");

                    b.ToTable("MailSubscriptions");
                });

            modelBuilder.Entity("LOVA.API.Models.MailType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MailTypes");
                });

            modelBuilder.Entity("LOVA.API.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ToDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("LOVA.API.Models.Premise", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Property")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("WellId")
                        .HasColumnType("bigint");

                    b.Property<string>("ZipCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("WellId");

                    b.ToTable("Premises");
                });

            modelBuilder.Entity("LOVA.API.Models.RentalInventory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BackgroundColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GroupItems")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOf")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("RentalInventories");
                });

            modelBuilder.Entity("LOVA.API.Models.RentalReservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AspNetUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOf")
                        .HasColumnType("int");

                    b.Property<DateTime>("PickupDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RentalInventoryId")
                        .HasColumnType("int");

                    b.Property<int>("RentalPaymentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("RentalInventoryId");

                    b.ToTable("RentalReservations");
                });

            modelBuilder.Entity("LOVA.API.Models.Survey", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Query1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Query10")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Query2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Query3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Query4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Query5")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Query6")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Query7")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Query8")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Query9")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SurveyName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Surveys");
                });

            modelBuilder.Entity("LOVA.API.Models.SurveyAnswered", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("SurveyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SurveyAnswereds");
                });

            modelBuilder.Entity("LOVA.API.Models.SurveyCheckbox", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("QueryNumber")
                        .HasColumnType("int");

                    b.Property<long>("SurveyId")
                        .HasColumnType("bigint");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SurveyId");

                    b.ToTable("SurveyCheckBoxes");
                });

            modelBuilder.Entity("LOVA.API.Models.UploadFileCategory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UploadFileCategories");
                });

            modelBuilder.Entity("LOVA.API.Models.UploadFileDirectory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Directory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UploadFileCategoryId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UploadFileCategoryId");

                    b.ToTable("UploadFileDirectories");
                });

            modelBuilder.Entity("LOVA.API.Models.UploadedFile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AspNetUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Container")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Directory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasDirectories")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDirectory")
                        .HasColumnType("bit");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("UploadFileCategoryId")
                        .HasColumnType("bigint");

                    b.Property<long>("UploadFileDirectoryId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("UploadedFiles");
                });

            modelBuilder.Entity("LOVA.API.Models.Well", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ActivatorSerialNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<double?>("Latitude")
                        .HasColumnType("float");

                    b.Property<double?>("Longitude")
                        .HasColumnType("float");

                    b.Property<int>("MasterNode")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ValveSerialNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WellName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Wells");
                });

            modelBuilder.Entity("LOVA.API.Models.WellMaintenanceWork", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPhoto")
                        .HasColumnType("bit");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("WellId")
                        .HasColumnType("bigint");

                    b.Property<string>("WorkBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkDescription")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("WellId");

                    b.ToTable("WellMaintenanceWorks");
                });

            modelBuilder.Entity("LOVA.API.Models.IssueReport", b =>
                {
                    b.HasOne("LOVA.API.Models.Well", "Well")
                        .WithMany("IssueReports")
                        .HasForeignKey("WellId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Well");
                });

            modelBuilder.Entity("LOVA.API.Models.Lova.LatestMaintenance", b =>
                {
                    b.HasOne("LOVA.API.Models.Lova.Maintenance", "Maintenance")
                        .WithMany("LatestMaintenances")
                        .HasForeignKey("MaintenanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Maintenance");
                });

            modelBuilder.Entity("LOVA.API.Models.MailSubscription", b =>
                {
                    b.HasOne("LOVA.API.Models.MailType", "MailType")
                        .WithMany("MailSubscriptions")
                        .HasForeignKey("MailTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MailType");
                });

            modelBuilder.Entity("LOVA.API.Models.Premise", b =>
                {
                    b.HasOne("LOVA.API.Models.Well", "Well")
                        .WithMany("Premises")
                        .HasForeignKey("WellId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Well");
                });

            modelBuilder.Entity("LOVA.API.Models.RentalReservation", b =>
                {
                    b.HasOne("LOVA.API.Models.RentalInventory", "RentalInventory")
                        .WithMany("RentalReservations")
                        .HasForeignKey("RentalInventoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RentalInventory");
                });

            modelBuilder.Entity("LOVA.API.Models.SurveyCheckbox", b =>
                {
                    b.HasOne("LOVA.API.Models.Survey", "Survey")
                        .WithMany("AreChecked1")
                        .HasForeignKey("SurveyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Survey");
                });

            modelBuilder.Entity("LOVA.API.Models.UploadFileDirectory", b =>
                {
                    b.HasOne("LOVA.API.Models.UploadFileCategory", "UploadFileCategory")
                        .WithMany()
                        .HasForeignKey("UploadFileCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UploadFileCategory");
                });

            modelBuilder.Entity("LOVA.API.Models.WellMaintenanceWork", b =>
                {
                    b.HasOne("LOVA.API.Models.Well", "Well")
                        .WithMany()
                        .HasForeignKey("WellId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Well");
                });

            modelBuilder.Entity("LOVA.API.Models.Lova.Maintenance", b =>
                {
                    b.Navigation("LatestMaintenances");
                });

            modelBuilder.Entity("LOVA.API.Models.MailType", b =>
                {
                    b.Navigation("MailSubscriptions");
                });

            modelBuilder.Entity("LOVA.API.Models.RentalInventory", b =>
                {
                    b.Navigation("RentalReservations");
                });

            modelBuilder.Entity("LOVA.API.Models.Survey", b =>
                {
                    b.Navigation("AreChecked1");
                });

            modelBuilder.Entity("LOVA.API.Models.Well", b =>
                {
                    b.Navigation("IssueReports");

                    b.Navigation("Premises");
                });
#pragma warning restore 612, 618
        }
    }
}
