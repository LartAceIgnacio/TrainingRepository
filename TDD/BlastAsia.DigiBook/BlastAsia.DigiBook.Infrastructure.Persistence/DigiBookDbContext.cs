using BlastAsia.DigiBook.Domain.Models.Contacts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence
{
    public class DigiBookDbContext : DbContext
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
        }
    }
}
