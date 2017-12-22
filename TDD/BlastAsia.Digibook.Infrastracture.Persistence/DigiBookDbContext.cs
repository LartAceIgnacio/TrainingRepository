using BlastAsia.Digibook.Domain.Models.Appointments;
using BlastAsia.Digibook.Domain.Models.Contacts;
using BlastAsia.Digibook.Domain.Models.Employees;
using BlastAsia.Digibook.Domain.Models.Venues;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.Digibook.Infrastracture.Persistence
{
    public class DigiBookDbContext:DbContext, IDigiBookDbContext
    {
        public DigiBookDbContext(
            DbContextOptions<DigiBookDbContext> options) : base(options) { }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<Venue> Venues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().Ignore(t => t.Photo);
        }
    }
}
