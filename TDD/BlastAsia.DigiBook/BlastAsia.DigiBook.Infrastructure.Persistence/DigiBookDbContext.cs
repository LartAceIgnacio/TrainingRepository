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
            :base(options)
        {

        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.
                 Entity<Employee>().Ignore(m => m.Photo);
        }
    }
}
