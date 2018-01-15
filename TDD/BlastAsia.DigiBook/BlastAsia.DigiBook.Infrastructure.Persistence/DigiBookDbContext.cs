using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Models.Venues;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using BlastAsia.DigiBook.Domain.Models.Security;
using BlastAsia.DigiBook.Infrastructure.Security;

namespace BlastAsia.DigiBook.Infrastructure.Persistence
{
    public class DigiBookDbContext
        :IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IDigiBookDbContext
    {
        public DigiBookDbContext(
            DbContextOptions<DigiBookDbContext> options)
            :base(options)
        {

        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Token> Tokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.
            //     Entity<Employee>().Ignore(m => m.Photo);
            //base.OnModelCreating(modelBuilder);
            base.OnModelCreating(modelBuilder);

            #region Appointments
            modelBuilder.Entity<Appointment>().ToTable("Appointment");
            modelBuilder.Entity<Appointment>().
            Property(a => a.AppointmentId)
            .ValueGeneratedOnAdd();
            modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Host)
            .WithMany(e => e.Appointments);
            modelBuilder.Entity<Appointment>()
            .HasOne(e => e.Guest)
            .WithMany(g => g.Appointments);
            #endregion

            #region Employees
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<Employee>()
            .Property(e => e.EmployeeId)
            .ValueGeneratedOnAdd();
            modelBuilder.Entity<Employee>()
            .HasMany(e => e.Appointments)
            .WithOne(e => e.Host);
            #endregion

            #region Contacts
            modelBuilder.Entity<Contact>().ToTable("Contact");
            modelBuilder.Entity<Contact>()
            .Property(c => c.ContactId)
            .ValueGeneratedOnAdd();
            modelBuilder.Entity<Contact>()
            .HasMany(c => c.Appointments)
            .WithOne(c => c.Guest);
            #endregion
        }
    }
}
