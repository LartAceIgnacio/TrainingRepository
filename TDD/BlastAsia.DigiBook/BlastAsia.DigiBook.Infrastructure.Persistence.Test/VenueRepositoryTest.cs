using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class VenueRepositoryTest
    {
        private Venue _venue;
        private string _connectionString;
        private DbContextOptions<DigiBookDbContext> _dbOptions;
        private DigiBookDbContext _dbContext;
        private VenueRepository _sut;

        [TestInitialize]
        public void Initialize()
        {
            _venue = new Venue()
            {
                VenueName = "Venue1",
                Description = "Sample Description for Venue 1."
            };


            _connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=DigiBookDb;Trusted_Connection=True;";
            _dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(_connectionString)
                .Options;

            _dbContext = new DigiBookDbContext(_dbOptions);

            _sut = new VenueRepository(_dbContext);
            _dbContext.Database.EnsureCreated();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _dbContext.Dispose();
            _dbContext = null;
        }

        [TestMethod]
        public void Create_WithValidVenueData_SavesRecordToDb()
        {   //error push tag first please go back  here later
            // Arrange
            // Act
            var result = _sut.Create(_venue);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(Guid.Empty, result.VenueId);

            // Cleanup
            _sut.Delete(_venue.VenueId);
        }

    }
}
