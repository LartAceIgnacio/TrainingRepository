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
        public Venue venue;
        public string connectionString;
        public DbContextOptions<DigiBookDbContext> dbOptions;
        public DigiBookDbContext dbContext;
        public VenueRepository sut;


        [TestInitialize]
        public void Initialize()
        {
            venue = new Venue {
                VenueName = "Office",
                Description = "Christmas Party"
            };

            connectionString = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";

            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new VenueRepository(dbContext);
        }


        [TestCleanup]
        public void TestCleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidDate_SavesRecordFromDb()
        {
            //Arrange
            

            //Act
            var newVenue = sut.Create(venue);

            //Assert
            Assert.IsNotNull(newVenue);
            Assert.IsTrue(newVenue.VenueId != Guid.Empty);

            //Cleanup
            sut.Delete(newVenue.VenueId);
        }


        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithExistingVenue_RemovesRecordInDb()
        {
            //Arrange
            var newVenue = sut.Create(venue);

            //Act
            sut.Delete(newVenue.VenueId);

            //Assert
            var found = sut.Retrieve(newVenue.VenueId);
            Assert.IsNull(found);
        }


        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingVenueId_ReturnsRecordFromDb()
        {
            //Arrange
            var newVenue = sut.Create(venue);

            //Act
            var found = sut.Retrieve(newVenue.VenueId);

            //Assert
            Assert.IsNotNull(found);

            //Cleanup
            sut.Delete(newVenue.VenueId);
        }


        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDatabase()
        {
            //Arrange
            var newVenue = sut.Create(venue);
            newVenue.VenueName = "Home";
            newVenue.Description = "Thanksgiving";

            //Act
            var updatedVenue = sut.Update(newVenue.VenueId, newVenue);

            //Assert
            Assert.AreEqual(updatedVenue.VenueName, newVenue.VenueName);
            Assert.AreEqual(updatedVenue.Description, newVenue.Description);

            //Cleanup
            sut.Delete(newVenue.VenueId);

        }
    }
}
