//using BlastAsia.DigiBook.Domain.Models;
//using BlastAsia.DigiBook.Domain.Models.Appointments;
//using BlastAsia.DigiBook.Domain.Models.Contacts;
//using BlastAsia.DigiBook.Domain.Models.Employees;
//using BlastAsia.DigiBook.Domain.Models.Venues;
//using Microsoft.EntityFrameworkCore;
//using System;

//namespace BlastAsia.DigiBook.Infrastructure.Persistence
//{
//    public class DigiBookDbContext
//        : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
//    {
//        public DigiBookDbContext(
//            DbContextOptions<DigiBookDbContext> options)
//            : base(options)
//        {

//        }
//        public DbSet<Contact> Contacts { get; set; }
//        public DbSet<Employee> Employees { get; set; }
//        public DbSet<Appointment> Appointments { get; set; }
//        public DbSet<Venue> Venues { get; set; }
//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//        }
//    }
//}