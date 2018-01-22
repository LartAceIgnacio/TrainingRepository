using BlastAsia.DigiBook.Domain.Flights;
using BlastAsia.DigiBook.Domain.Models.Flights;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class FlightRepositoryTest
    {
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private String connectionString = null;
        private FlightRepository sut = null;
        private Flight flight = null;

        [TestInitialize]
        public void InitializeTest()
        {
            connectionString
                = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";

            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new FlightRepository(dbContext);

            flight = new Flight
            {
                CityOfOrigin = "123",
                CityOfDestination = "123",
                ExpectedTimeOfArrival = DateTime.Now.AddHours(1),
                ExpectedTimeOfDeparture = DateTime.Now.AddHours(1),
                FlightCode = "OOODDDYYMMddNN",
                DateCreated = new Nullable<DateTime>(),
                DateModified = new Nullable<DateTime>()
            };
        }
        [TestCleanup]
        public void CleanupTest()
        {
            dbContext.Dispose();
            dbContext = null;
        }
        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordInDatabase()
        {
            // Arrange



            // Act

            var newFlight = sut.Create(flight);

            // Assert

            Assert.IsNotNull(newFlight);
            Assert.IsTrue(newFlight.FlightId != Guid.Empty);

            // Cleanup

            sut.Delete(newFlight.FlightId);
        }
        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithExistingFlight_RemovesRecordFromDatabase()
        {
            // Arrange

            var newFlight = sut.Create(flight);

            // Act

            sut.Delete(newFlight.FlightId);

            // Assert

            flight = sut.Retrieve(newFlight.FlightId);
            Assert.IsNull(flight);
        }
        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingFlightId_ReturnsRecordsFromDatabase()
        {
            // Arrange

            var newFlight = sut.Create(flight);

            // Act

            var found = sut.Retrieve(newFlight.FlightId);

            // Assert

            Assert.IsNotNull(found);

            // Cleanup

            sut.Delete(found.FlightId);
        }
        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdateInDatabase()
        {
            // Arrange

            var newFlight = sut.Create(flight);
            var expectedCityOfOrigin = "321";
            var expectedCityOfDestination = "321";
            var expectedETA = DateTime.Now.AddHours(2);
            var expectedETD = DateTime.Now.AddHours(2);
            var expectedDateModified = DateTime.Now;

            flight.CityOfOrigin = expectedCityOfOrigin;
            flight.CityOfDestination = expectedCityOfDestination;
            flight.ExpectedTimeOfArrival = expectedETA;
            flight.ExpectedTimeOfDeparture = expectedETD;
            flight.DateModified = expectedDateModified;

            // Act

            sut.Update(newFlight.FlightId, newFlight);

            // Assert

            var updatedFlight = sut.Retrieve(newFlight.FlightId);
            Assert.AreEqual(expectedCityOfOrigin, updatedFlight.CityOfOrigin);
            Assert.AreEqual(expectedCityOfDestination, updatedFlight.CityOfDestination);
            Assert.AreEqual(expectedETA, updatedFlight.ExpectedTimeOfArrival);
            Assert.AreEqual(expectedETD, updatedFlight.ExpectedTimeOfDeparture);
            Assert.AreEqual(expectedDateModified, updatedFlight.DateModified);

            // Cleanup

            sut.Delete(updatedFlight.FlightId);
        }
    }
}
