﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MinimalApi.Migrations
{
    [DbContext(typeof(RaceDb))]
    [Migration("20220620131124_Motorbike_persistence")]
    partial class Motorbike_persistence
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("DistanceCoverdInMiles")
                        .HasColumnType("int");

                    b.Property<bool>("FinishedRace")
                        .HasColumnType("bit");

                    b.Property<double>("MelfunctionChance")
                        .HasColumnType("float");

                    b.Property<int>("MelfunctionsOccured")
                        .HasColumnType("int");

                    b.Property<int>("RacedForHours")
                        .HasColumnType("int");

                    b.Property<int>("Speed")
                        .HasColumnType("int");

                    b.Property<string>("TeamName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("Motorbike", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("DistanceCoverdInMiles")
                        .HasColumnType("int");

                    b.Property<bool>("FinishedRace")
                        .HasColumnType("bit");

                    b.Property<double>("MelfunctionChance")
                        .HasColumnType("float");

                    b.Property<int>("MelfunctionsOccured")
                        .HasColumnType("int");

                    b.Property<int>("RacedForHours")
                        .HasColumnType("int");

                    b.Property<int>("Speed")
                        .HasColumnType("int");

                    b.Property<string>("TeamName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Motorbikes");
                });
#pragma warning restore 612, 618
        }
    }
}
