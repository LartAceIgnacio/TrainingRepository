using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence
{
    public class DigiBookDbContext : DbContext, IDigiBookDbContext
    {
        public DbSet<Contact> Contacts { get; set; }

        public DigiBookDbContext(DbContextOptions<DigiBookDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>()
                .ToTable("Contact")
                .HasKey(k => k.ContactId);

            modelBuilder.Entity<Employee>()
                .Ignore(i => i.Photo)
                .ToTable("Employee")
                .HasKey(k => k.Id);

            modelBuilder.Entity<Appointment>()
                .ToTable("Appointment")
                .HasKey(a => a.AppointmentId);
        }
        
    }
}
