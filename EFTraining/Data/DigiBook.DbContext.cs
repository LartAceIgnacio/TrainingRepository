using System;
using EFTraining.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EFTraining.Data
{
    public class DigiBookDbContext : DbContext
    {
        public DigiBookDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Appointments

            modelBuilder.Entity<Appointment>()
            .ToTable("Appointmnet");
            modelBuilder.Entity<Appointment>()
             .Property(a => a.AppointmentId)
             .ValueGeneratedOnAdd();
            modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Host)
            .WithMany(e => e.Appointments);
            modelBuilder.Entity<Appointment>()
             .HasOne(a => a.Guest)
             .WithMany(e => e.Appointments);

             #endregion

             #region Employees
            
            modelBuilder.Entity<Employee>()
            .ToTable("Employee");
            modelBuilder.Entity<Employee>()
            .Property(a => a.EmployeeId)
            .ValueGeneratedOnAdd();
            modelBuilder.Entity<Employee>()
            .HasMany(e => e.Appointments)
            .WithOne( a => a.Host);
            
             #endregion

              #region Contacts
            
            modelBuilder.Entity<Contact>()
            .ToTable("Contact");
            modelBuilder.Entity<Contact>()
            .Property(a => a.ContactId)
            .ValueGeneratedOnAdd();
            modelBuilder.Entity<Contact>()
            .HasMany(e => e.Appointments)
            .WithOne( a => a.Guest);
            
             #endregion
        }
    }
}