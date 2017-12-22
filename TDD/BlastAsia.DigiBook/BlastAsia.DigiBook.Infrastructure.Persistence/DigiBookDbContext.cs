using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Departments;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Models.Venues;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence
{
    public class DigiBookDbContext
        : DbContext, IDigiBookDbContext
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
            modelBuilder.Entity<Contact>()
                .ToTable("Contact")
                .HasKey(KeyExtensions => KeyExtensions.ContactId);

            modelBuilder.Entity<Employee>()
                .Ignore(t => t.Photo)
                .ToTable("Employee")
                .HasKey(KeyExtensions => KeyExtensions.EmployeeId);

            modelBuilder.Entity<Appointment>()
                .ToTable("Appointment")
                .HasKey(KeyExtensions => KeyExtensions.AppointmentId);

            modelBuilder.Entity<Department>()
                .ToTable("Department")
                .HasKey(KeyExtensions => KeyExtensions.DepartmentId);

            modelBuilder.Entity<Venue>()
                .ToTable("Venue")
                .HasKey(KeyExtensions => KeyExtensions.VenueId);
        }
    }
}
