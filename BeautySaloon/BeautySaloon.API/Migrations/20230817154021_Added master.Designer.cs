﻿// <auto-generated />
using System;
using BeautySaloon.API.Entities.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BeautySaloon.API.Migrations
{
    [DbContext(typeof(BeautySaloonContext))]
    [Migration("20230817154021_Added master")]
    partial class Addedmaster
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BeautySaloon.API.Entities.BeautySaloonContextEntities.BestWork", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<string>("ImageBucket")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageFileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BestWorks");
                });

            modelBuilder.Entity("BeautySaloon.API.Entities.BeautySaloonContextEntities.Comment", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rate")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("BeautySaloon.API.Entities.BeautySaloonContextEntities.Customer", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("BeautySaloon.API.Entities.BeautySaloonContextEntities.DayOff", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("MasterId")
                        .HasColumnType("int");

                    b.Property<int?>("ScheduleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MasterId");

                    b.HasIndex("ScheduleId");

                    b.ToTable("DayOff");
                });

            modelBuilder.Entity("BeautySaloon.API.Entities.BeautySaloonContextEntities.Master", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ScheduleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleId");

                    b.ToTable("Masters");
                });

            modelBuilder.Entity("BeautySaloon.API.Entities.BeautySaloonContextEntities.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ServiceId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("BeautySaloon.API.Entities.BeautySaloonContextEntities.Schedule", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.HasKey("Id");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("BeautySaloon.API.Entities.BeautySaloonContextEntities.Service", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<int?>("MasterId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("MasterId");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("BeautySaloon.API.Entities.BeautySaloonContextEntities.ServiceCategory", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageBucket")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageFileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ServiceCategories");
                });

            modelBuilder.Entity("BeautySaloon.API.Entities.BeautySaloonContextEntities.WorkingDay", b =>
                {
                    b.Property<int>("WorkingDayId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WorkingDayId"));

                    b.Property<string>("Day")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<int>("ScheduleId")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.HasKey("WorkingDayId");

                    b.HasIndex("ScheduleId");

                    b.ToTable("WorkingDays");
                });

            modelBuilder.Entity("BeautySaloon.API.Entities.BeautySaloonContextEntities.DayOff", b =>
                {
                    b.HasOne("BeautySaloon.API.Entities.BeautySaloonContextEntities.Master", "Master")
                        .WithMany()
                        .HasForeignKey("MasterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeautySaloon.API.Entities.BeautySaloonContextEntities.Schedule", null)
                        .WithMany("DayOffs")
                        .HasForeignKey("ScheduleId");

                    b.Navigation("Master");
                });

            modelBuilder.Entity("BeautySaloon.API.Entities.BeautySaloonContextEntities.Master", b =>
                {
                    b.HasOne("BeautySaloon.API.Entities.BeautySaloonContextEntities.Schedule", "Schedule")
                        .WithMany()
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("BeautySaloon.API.Entities.BeautySaloonContextEntities.Reservation", b =>
                {
                    b.HasOne("BeautySaloon.API.Entities.BeautySaloonContextEntities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeautySaloon.API.Entities.BeautySaloonContextEntities.Service", "Service")
                        .WithMany("Reservations")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("BeautySaloon.API.Entities.BeautySaloonContextEntities.Service", b =>
                {
                    b.HasOne("BeautySaloon.API.Entities.BeautySaloonContextEntities.ServiceCategory", "Category")
                        .WithMany("Services")
                        .HasForeignKey("CategoryId");

                    b.HasOne("BeautySaloon.API.Entities.BeautySaloonContextEntities.Master", null)
                        .WithMany("Services")
                        .HasForeignKey("MasterId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("BeautySaloon.API.Entities.BeautySaloonContextEntities.WorkingDay", b =>
                {
                    b.HasOne("BeautySaloon.API.Entities.BeautySaloonContextEntities.Schedule", null)
                        .WithMany("WorkingDays")
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BeautySaloon.API.Entities.BeautySaloonContextEntities.Master", b =>
                {
                    b.Navigation("Services");
                });

            modelBuilder.Entity("BeautySaloon.API.Entities.BeautySaloonContextEntities.Schedule", b =>
                {
                    b.Navigation("DayOffs");

                    b.Navigation("WorkingDays");
                });

            modelBuilder.Entity("BeautySaloon.API.Entities.BeautySaloonContextEntities.Service", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("BeautySaloon.API.Entities.BeautySaloonContextEntities.ServiceCategory", b =>
                {
                    b.Navigation("Services");
                });
#pragma warning restore 612, 618
        }
    }
}
