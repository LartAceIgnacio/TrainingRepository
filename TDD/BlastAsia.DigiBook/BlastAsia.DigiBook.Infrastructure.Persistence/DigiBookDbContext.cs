using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Departments;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Models.Security;
using BlastAsia.DigiBook.Domain.Models.Venues;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence
{
    public class DigiBookDbContext
        :IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IDigiBookDbContext
    {

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Venue> Venue { get; set; }

        public DigiBookDbContext(
            DbContextOptions<DigiBookDbContext> options)
            : base(options)
        {

        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Contact>()
            //    .ToTable("Contact")
            //    .HasKey(KeyExtensions => KeyExtensions.ContactId);

            //modelBuilder.Entity<Employee>()
            //    .Ignore(t => t.Photo)
            //    .ToTable("Employee")
            //    .HasKey(KeyExtensions => KeyExtensions.EmployeeId);

            //modelBuilder.Entity<Appointment>()
            //    .ToTable("Appointment")
            //    .HasKey(KeyExtensions => KeyExtensions.AppointmentId);

            //modelBuilder.Entity<Department>()
            //    .ToTable("Department")
            //    .HasKey(KeyExtensions => KeyExtensions.DepartmentId);

            //modelBuilder.Entity<Venue>()
            //    .ToTable("Venue")
            //    .HasKey(KeyExtensions => KeyExtensions.VenueId);

            #region Appointments
            modelBuilder.Entity<Appointment>().ToTable("Appointment");
            modelBuilder.Entity<Appointment>()
                .Property(a => a.AppointmentId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Host)
                .WithMany(e => e.Appointments);
            modelBuilder.Entity<Appointment>()
                .HasOne(e => e.Guest)
                .WithMany(g => g.Appointments);
            #endregion

            #region Employees
            modelBuilder.Entity<Employee>()
                .Ignore(t => t.Photo)
                .ToTable("Employee");
            modelBuilder.Entity<Employee>()
                .Property(e => e.EmployeeId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Appointments)
                .WithOne(a => a.Host);
            #endregion

            #region Contacts
            modelBuilder.Entity<Contact>().ToTable("Contact");
            modelBuilder.Entity<Contact>()
                .Property(c => c.ContactId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Contact>()
                .HasMany(c => c.Appointments)
                .WithOne(a => a.Guest);
            #endregion
        }
    }
}
