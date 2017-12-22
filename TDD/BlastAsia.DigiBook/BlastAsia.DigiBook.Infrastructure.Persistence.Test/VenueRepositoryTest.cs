using BlastAsia.DigiBook.Domain.Models.Venues;
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

        private Venue venue;
        private string connectionString;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private VenueRepository sut;
        private DigiBookDbContext dbContext;
       
        [TestInitialize()]
        public void Initialize()
        {
            venue = new Venue
            {
                VenueName = "New Venue",
                Description = "For New Venue"
            };

            connectionString =
               @"Data Source=.; Database=DigiBookDb; Integrated Security=true";

            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();
            sut = new VenueRepository(dbContext);


        }
        [TestCleanup()]
        public void CleanUp()
        {

        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SaveRecordInTheDatabase()
        {
            //Arrange

            //Act
            var newVenue = sut.Create(venue);

            //Assert
            Assert.IsNotNull(venue);
            Assert.IsTrue(newVenue.VenueId != Guid.Empty);

            //CleanUp
            sut.Delete(venue.VenueId);

        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingVenue_RemovesRecordFromDatabase()
        {
            var newVenue = sut.Create(venue);
            //Act
            sut.Delete(newVenue.VenueId);

            venue = sut.Retrieve(newVenue.VenueId);
            Assert.IsNull(venue);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingVenueId_ReturnsRecordFromDb()
        {

            var newVenue = sut.Create(venue);

            var found = sut.Retrieve(newVenue.VenueId);

            Assert.IsNotNull(found);
            sut.Delete(newVenue.VenueId);

        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SaveUpdatesInDb()
        {
            //ARRANGE
            var venueData = sut.Create(venue);
            var expectedVenueName = "somewhere";
            var expectedVenueDescription = "sasdsada";

            var newVenue = sut.Retrieve(venueData.VenueId);
            newVenue.VenueName = expectedVenueName;
            newVenue.Description = expectedVenueDescription;

            sut.Update(newVenue.VenueId, newVenue);

            //ACT
            var updatedVenue = sut.Retrieve(newVenue.VenueId);
            Assert.AreEqual(expectedVenueName, updatedVenue.VenueName);
            Assert.AreEqual(expectedVenueDescription, updatedVenue.Description);

            //CleanUp
            sut.Delete(updatedVenue.VenueId);
        }
    }
}
