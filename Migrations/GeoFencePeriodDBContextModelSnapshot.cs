﻿// <auto-generated />
using System;
using GeoFence_VehicleNonAvailability.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GeoFence_VehicleNonAvailability.Migrations
{
    [DbContext(typeof(GeoFencePeriodDBContext))]
    partial class GeoFencePeriodDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GeoFence_VehicleNonAvailability.Models.Domain.GeoFencePeriod", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("entertime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("exittime")
                        .HasColumnType("datetime2");

                    b.Property<int>("vehicleid")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("GeoFencePeriods");
                });
#pragma warning restore 612, 618
        }
    }
}
