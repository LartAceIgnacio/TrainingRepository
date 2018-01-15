using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Departments;
using BlastAsia.DigiBook.Domain.Models.Employees;
using BlastAsia.DigiBook.Domain.Models.Venues;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BlastAsia.DigiBook.Infrastructure.Security;

namespace BlastAsia.DigiBook.Infrastructure.Persistence
{
    public class DigiBookDbContext 
        : IdentityDbContext<ApplicationUser, ApplicationRole, Guid,
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>, IDigiBookDbContext
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

        public DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            #region Appointments

            modelBuilder.Entity<Appointment>().ToTable("Appointment");
            modelBuilder.Entity<Appointment>()
                .Property(a => a.AppointmentId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Appointment>().
                HasOne(a => a.Host)
                .WithMany(e => e.Appointments);
            modelBuilder.Entity<Appointment>().
                HasOne(e => e.Guest)
                .WithMany(g => g.Appointments);
            #endregion

            #region Employees

            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<Employee>()
                .Property(e => e.EmployeeId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Appointments)
                .WithOne(a => a.Host);
            #endregion

            #region Contacts
            modelBuilder.Entity<Contact>().ToTable("Contact");
            modelBuilder.Entity<Contact>()
                .Property(c => c.ContactId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Contact>()
                .HasMany(c => c.Appointments)
                .WithOne(a => a.Guest);
            #endregion

            //modelBuilder.Entity<Venue>().ToTable("Venue");

            //modelBuilder.Entity<Contact>().ToTable("Contact");

            modelBuilder.Entity<Employee>().Ignore(t => t.Photo);

            //modelBuilder.Entity<Employee>().ToTable("Employee");

            //modelBuilder.Entity<Appointment>().ToTable("Appointment");

            //modelBuilder.Entity<Department>().ToTable("Department");

            //modelBuilder.Entity<Venue>().ToTable("Venue");

        }
    }
}
