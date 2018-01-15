using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Infrastructure.Persistence.Venues;
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
        private Venue venue = null;
        private string connectionString= null;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private VenueRepository sut = null;

        [TestInitialize]
        public void Initialize()
        {
            venue = new Venue
            {
                VenueName = "Training",
                Description = "Final test"
            };
            connectionString =
                @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";


            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new VenueRepository(dbContext);
        }

        [TestCleanup]
        public void CleanUp()
        {
            dbContext.Dispose();
            dbContext = null;
        }



        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithvalidData_SaveRecordsIntheDatabase()
        {

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
        public void Delte_WithAnExsitingVenue_RemovesRecordFromData()
        {
            //Arrange
            var newVenue = sut.Create(venue);

            //Act
            sut.Delete(newVenue.VenueId);

            //Assert
            venue = sut.Retrieve(newVenue.VenueId);
            Assert.IsNull(venue);
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

            //cleanup
            sut.Delete(found.VenueId);

        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDb()
        {
            //Arrange
            var newVenue = sut.Create(venue);
            var expectedVenueName = "Trainning";
            var expectedDescription = "Final training in tdd";

            newVenue.VenueName = expectedVenueName;
            newVenue.Description = expectedDescription;

            //Act
            var result = sut.Update(newVenue.VenueId, venue);

            //Assert
            var updatedVenue = sut.Retrieve(newVenue.VenueId);
            Assert.AreEqual(expectedVenueName, newVenue.VenueName);
            Assert.AreEqual(expectedDescription, newVenue.Description);

            //CleanUp
            sut.Delete(updatedVenue.VenueId);

        }
    }
}
