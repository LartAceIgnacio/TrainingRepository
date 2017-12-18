using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Employees;
using Microsoft.EntityFrameworkCore;

namespace BlastAsia.DigiBook.Infrastructure.Persistence
{
    public class DigiBookDbContext
        :DbContext, IDigiBookDbContext
    {
        public DigiBookDbContext(
            DbContextOptions<DigiBookDbContext> options)
            : base(options)
        {

        }

        public DbSet <Contact> Contacts { get; set; }
        public DbSet <Employee> Employee { get; set; }
        public DbSet <Appointment> Appointment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>().ToTable("Contact");

            modelBuilder.Entity<Employee>().Ignore(t => t.Photo);
            modelBuilder.Entity<Employee>().ToTable("Employee");

            modelBuilder.Entity<Appointment>().ToTable("Appointment");
        }
    }
}
