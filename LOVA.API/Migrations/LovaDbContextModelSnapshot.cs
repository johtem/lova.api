﻿// <auto-generated />
using System;
using LOVA.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LOVA.API.Migrations
{
    [DbContext(typeof(LovaDbContext))]
    partial class LovaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LOVA.API.Models.DrainPatrol", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<int>("Master_node")
                        .HasColumnType("int");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<long>("WellId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("WellId");

                    b.ToTable("DrainPatrols");
                });

            modelBuilder.Entity("LOVA.API.Models.IssueReport", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Alarm")
                        .HasColumnType("int");

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

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("WellId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("WellId");

                    b.ToTable("IssueReports");
                });

            modelBuilder.Entity("LOVA.API.Models.Premise", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Property")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("WellId")
                        .HasColumnType("bigint");

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

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ValveSerialNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WellName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Wells");
                });

            modelBuilder.Entity("LOVA.API.Models.DrainPatrol", b =>
                {
                    b.HasOne("LOVA.API.Models.Well", "Well")
                        .WithMany()
                        .HasForeignKey("WellId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LOVA.API.Models.IssueReport", b =>
                {
                    b.HasOne("LOVA.API.Models.Well", "Well")
                        .WithMany("IssueReports")
                        .HasForeignKey("WellId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LOVA.API.Models.Premise", b =>
                {
                    b.HasOne("LOVA.API.Models.Well", "Well")
                        .WithMany("Premises")
                        .HasForeignKey("WellId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LOVA.API.Models.RentalReservation", b =>
                {
                    b.HasOne("LOVA.API.Models.RentalInventory", "RentalInventory")
                        .WithMany("RentalReservations")
                        .HasForeignKey("RentalInventoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
