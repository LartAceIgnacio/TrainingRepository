using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Infrastracture.Persistence;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlastAsia.DigiBook.Insfrastracture.Persistence
{
    public class DigiBookDbContext
        : DbContext, IDigiBookDbContext
    {
        public DigiBookDbContext(DbContextOptions<DigiBookDbContext> options)
            :base (options)
        {

        }
        //tables
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().Ignore(x => x.Photo);
        } 
    }
}
