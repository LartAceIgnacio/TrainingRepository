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
        :DbContext, IDigiBookDbContext
    {
        public DigiBookDbContext(
            DbContextOptions<DigiBookDbContext>options)
            :base(options)
        {

        }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Venue> Venue { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>().ToTable("Contact");

            modelBuilder.Entity<Employee>().Ignore(t => t.Photo);

            modelBuilder.Entity<Employee>().ToTable("Employee");

            modelBuilder.Entity<Appointment>().ToTable("Appointment");

            modelBuilder.Entity<Department>().ToTable("Department");

            modelBuilder.Entity<Venue>().ToTable("Venue");

        }
    }
}
