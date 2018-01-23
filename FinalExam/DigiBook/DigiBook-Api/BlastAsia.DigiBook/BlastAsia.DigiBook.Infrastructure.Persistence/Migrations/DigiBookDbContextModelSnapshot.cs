﻿// <auto-generated />
using BlastAsia.DigiBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(DigiBookDbContext))]
    partial class DigiBookDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BlastAsia.DigiBook.Domain.Models.Appointments.Appointment", b =>
                {
                    b.Property<Guid>("AppointmentId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("AppointmentDate");

                    b.Property<TimeSpan?>("EndTime");

                    b.Property<Guid>("GuestId");

                    b.Property<Guid>("HostId");

                    b.Property<bool>("IsCancelled");

                    b.Property<bool>("IsDone");

                    b.Property<string>("Notes");

                    b.Property<TimeSpan?>("StartTime");

                    b.HasKey("AppointmentId");

                    b.HasIndex("GuestId");

                    b.HasIndex("HostId");

                    b.ToTable("Appointment");
                });

            modelBuilder.Entity("BlastAsia.DigiBook.Domain.Models.Contacts.Contact", b =>
                {
                    b.Property<Guid>("ContactId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CityAddress");

                    b.Property<string>("Country");

                    b.Property<DateTime?>("DateActivated");

                    b.Property<string>("EmailAddress");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsActive");

                    b.Property<string>("LastName");

                    b.Property<string>("MobilePhone");

                    b.Property<string>("StreetAddress");

                    b.Property<int>("ZipCode");

                    b.HasKey("ContactId");

                    b.ToTable("Contact");
                });

            modelBuilder.Entity("BlastAsia.DigiBook.Domain.Models.Employees.Employee", b =>
                {
                    b.Property<Guid>("EmployeeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EmailAddress");

                    b.Property<string>("Extension");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("MobilePhone");

                    b.Property<string>("OfficePhone");

                    b.Property<string>("Photo");

                    b.HasKey("EmployeeId");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("BlastAsia.DigiBook.Domain.Models.Pilots.Pilot", b =>
                {
                    b.Property<Guid>("PilotId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateActivated");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("MiddleName");

                    b.Property<string>("PilotCode");

                    b.Property<int?>("YearsOfExperience");

                    b.HasKey("PilotId");

                    b.ToTable("Pilot");
                });

            modelBuilder.Entity("BlastAsia.DigiBook.Domain.Models.Venues.Venue", b =>
                {
                    b.Property<Guid>("VenueId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("VenueName");

                    b.HasKey("VenueId");

                    b.ToTable("Venue");
                });

            modelBuilder.Entity("BlastAsia.DigiBook.Infrastructure.Security.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("BlastAsia.DigiBook.Infrastructure.Security.ApplicationRoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("BlastAsia.DigiBook.Infrastructure.Security.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("DisplayName");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<DateTime>("LastModifiedDate");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BlastAsia.DigiBook.Infrastructure.Security.ApplicationUserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("BlastAsia.DigiBook.Infrastructure.Security.ApplicationUserLogin", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("BlastAsia.DigiBook.Infrastructure.Security.ApplicationUserRole", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("BlastAsia.DigiBook.Infrastructure.Security.ApplicationUserToken", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("BlastAsia.DigiBook.Infrastructure.Security.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClientId")
                        .IsRequired();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("Type");

                    b.Property<Guid>("UserId");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("BlastAsia.DigiBook.Domain.Models.Appointments.Appointment", b =>
                {
                    b.HasOne("BlastAsia.DigiBook.Domain.Models.Contacts.Contact", "Guest")
                        .WithMany("Appointments")
                        .HasForeignKey("GuestId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BlastAsia.DigiBook.Domain.Models.Employees.Employee", "Host")
                        .WithMany("Appointments")
                        .HasForeignKey("HostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BlastAsia.DigiBook.Infrastructure.Security.ApplicationRoleClaim", b =>
                {
                    b.HasOne("BlastAsia.DigiBook.Infrastructure.Security.ApplicationRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BlastAsia.DigiBook.Infrastructure.Security.ApplicationUserClaim", b =>
                {
                    b.HasOne("BlastAsia.DigiBook.Infrastructure.Security.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BlastAsia.DigiBook.Infrastructure.Security.ApplicationUserLogin", b =>
                {
                    b.HasOne("BlastAsia.DigiBook.Infrastructure.Security.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BlastAsia.DigiBook.Infrastructure.Security.ApplicationUserRole", b =>
                {
                    b.HasOne("BlastAsia.DigiBook.Infrastructure.Security.ApplicationRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BlastAsia.DigiBook.Infrastructure.Security.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BlastAsia.DigiBook.Infrastructure.Security.ApplicationUserToken", b =>
                {
                    b.HasOne("BlastAsia.DigiBook.Infrastructure.Security.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BlastAsia.DigiBook.Infrastructure.Security.Token", b =>
                {
                    b.HasOne("BlastAsia.DigiBook.Infrastructure.Security.ApplicationUser", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
