﻿// <auto-generated />
using EFTraining.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace EFTraining.Data.Migrations
{
    [DbContext(typeof(DigiBookDbContext))]
    [Migration("20180109053222_Initial Migration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EFTraining.Data.Models.Appointment", b =>
                {
                    b.Property<Guid>("AppointmentId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AppointmentDate");

                    b.Property<DateTime>("DateCreated");

                    b.Property<Guid?>("GuestContactId");

                    b.Property<Guid?>("HostEmployeeId");

                    b.HasKey("AppointmentId");

                    b.HasIndex("GuestContactId");

                    b.HasIndex("HostEmployeeId");

                    b.ToTable("Appointmnet");
                });

            modelBuilder.Entity("EFTraining.Data.Models.Contact", b =>
                {
                    b.Property<Guid>("ContactId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Firstname");

                    b.Property<string>("Lastname");

                    b.Property<string>("MobilePhone");

                    b.HasKey("ContactId");

                    b.ToTable("Contact");
                });

            modelBuilder.Entity("EFTraining.Data.Models.Employee", b =>
                {
                    b.Property<Guid>("EmployeeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Firstname");

                    b.Property<string>("Lastname");

                    b.Property<string>("OfficePhone");

                    b.HasKey("EmployeeId");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("EFTraining.Data.Models.Appointment", b =>
                {
                    b.HasOne("EFTraining.Data.Models.Contact", "Guest")
                        .WithMany("Appointments")
                        .HasForeignKey("GuestContactId");

                    b.HasOne("EFTraining.Data.Models.Employee", "Host")
                        .WithMany("Appointments")
                        .HasForeignKey("HostEmployeeId");
                });
#pragma warning restore 612, 618
        }
    }
}
