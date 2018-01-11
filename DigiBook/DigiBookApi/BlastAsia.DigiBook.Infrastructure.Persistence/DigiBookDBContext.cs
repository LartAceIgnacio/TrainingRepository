using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Models.Venues;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BlastAsia.DigiBook.Infrastructure.Persistence
{
    public class DigiBookDbContext: IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IDigiBookDbContext
    {
        public DigiBookDbContext(DbContextOptions<DigiBookDbContext> options)
            :base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Venue> Venues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Appointments
            modelBuilder.Entity<Appointment>()
                .ToTable("Appointment");
            modelBuilder.Entity<Appointment>()
                .Property(a => a.AppointmentId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Employee)
                .WithMany(a => a.Appointments);
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Contact)
                .WithMany(a => a.Appointments);
            #endregion

            #region Contacts
            modelBuilder.Entity<Contact>()
                .ToTable("Contact");
            modelBuilder.Entity<Contact>()
                .Property(c => c.ContactId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Contact>()
                .HasMany(c => c.Appointments)
                .WithOne(c => c.Contact);
            #endregion

            #region Employees
            modelBuilder.Entity<Employee>().Ignore(t => t.Photo);
            modelBuilder.Entity<Employee>()
                .ToTable("Employee");
            modelBuilder.Entity<Employee>()
                .Property(e => e.EmployeeId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Appointments)
                .WithOne(e => e.Employee);
            #endregion

            modelBuilder.Entity<Venue>().ToTable("Venue");
        }
    }
}
