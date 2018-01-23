using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Departments;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Models.Pilots;
//using BlastAsia.DigiBook.Domain.Models.Security;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Infrastracture.Persistence;
using BlastAsia.DigiBook.Infrastructure.Security;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlastAsia.DigiBook.Insfrastracture.Persistence
{
    public class DigiBookDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IDigiBookDbContext
    {
        public DigiBookDbContext(DbContextOptions<DigiBookDbContext> options)
            :base (options)
        {

        }
        //tables
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Pilot> Pilots { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Token
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<ApplicationUser>().HasMany(u => u.Tokens).WithOne(i => i.User);

            modelBuilder.Entity<ApplicationRole>().ToTable("Roles");

            modelBuilder.Entity<ApplicationRoleClaim>().ToTable("RoleClaims");

            modelBuilder.Entity<ApplicationUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<ApplicationUserToken>().ToTable("UserTokens");


            modelBuilder.Entity<Token>().ToTable("Tokens");
            modelBuilder.Entity<Token>().Property(i => i.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Token>().HasOne(i => i.User).WithMany(u => u.Tokens);
            #endregion

            #region Appointments

            modelBuilder.Entity<Appointment>().ToTable("Appointments");
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

            modelBuilder.Entity<Employee>().ToTable("Employees");

            modelBuilder.Entity<Employee>()
                .Property(e => e.EmployeeId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Appointments)
                .WithOne(a => a.Host);

            modelBuilder.Entity<Employee>().Ignore(x => x.Photo);
            #endregion


            #region Employee

            modelBuilder.Entity<Contact>().ToTable("Contacts");

            modelBuilder.Entity<Contact>()
                .Property(e => e.ContactId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Contact>()
                .HasMany(e => e.Appointments)
                .WithOne(a => a.Guest);

            #endregion

            modelBuilder.Entity<Venue>().ToTable("Venues");
            modelBuilder.Entity<Pilot>().ToTable("Pilots");


        }
    }
}
