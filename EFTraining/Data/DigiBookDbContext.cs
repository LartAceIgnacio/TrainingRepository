using System;
using EFTraining.Data.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace EFTraining.Data
{
    public class DigiBookDbContext : DbContext
    {
        public DigiBookDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Appointments

            modelBuilder.Entity<Appointment>().ToTable("Appointment");
            // Generate id
            modelBuilder.Entity<Appointment>()
                .Property(a => a.AppointmentId)
                .ValueGeneratedOnAdd();
            // relationship to host
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Host)
                .WithMany(e => e.Appointments);
            // relationship to guest
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Guest)
                .WithMany(g => g.Appointments);

            #endregion


            #region Employee

            modelBuilder.Entity<Employee>().ToTable("Employee");

            modelBuilder.Entity<Employee>()
                .Property(e => e.EmployeeId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Appointments)
                .WithOne(a => a.Host);

            #endregion

            
            #region Employee

            modelBuilder.Entity<Contact>().ToTable("Contact");
            
            modelBuilder.Entity<Contact>()
                .Property(e => e.ContactId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Contact>()
                .HasMany(e => e.Appointments)
                .WithOne(a => a.Guest);

            #endregion
        }

    }
}
