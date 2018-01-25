using BlastAsia.DigiBook.Domain.Models.Flights;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.Digibook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class FlightRepositoryTest
    {
        private Flight flight;
        private DbContextOptions<DigiBookDbContext> dbOptions;
        private DigiBookDbContext dbContext;
        private String connectionString;
        private FlightRepository sut;

        [TestInitialize]
        public void Initialize()
        {
            flight = new Flight
            {
                CityOfOrigin = "MNL",
                CityOfDestination = "LGN",
                Eta = DateTime.Now.AddHours(2),
                Etd = DateTime.Now.AddHours(3)
            };

            connectionString =
                @"Server=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new FlightRepository(dbContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_FlightWithValidData_SavesRecordInDatabase()
        {
            // Act
            var newFlight = sut.Create(flight);

            // Assert
            Assert.IsNotNull(newFlight);
            Assert.IsTrue(newFlight.FlightId != Guid.Empty);

            //Cleanup
            sut.Delete(flight.FlightId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingFlight_RemovesRecordFromDb()
        {
            // Arrange
            var newFlight = sut.Create(flight);

            // Act
            sut.Delete(newFlight.FlightId);

            // Assert
            flight = sut.Retrieve(flight.FlightId);
            Assert.IsNull(flight);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingFlightId_ReturnsRecordFromDb()
        {
            // Arrange
            var newFlight = sut.Create(flight);

            // Act
            var found = sut.Retrieve(newFlight.FlightId);

            // Assert
            Assert.IsNotNull(found);
            sut.Delete(found.FlightId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdateInDb()
        {
            // Arrange
            var newFlight = sut.Create(flight);
            var expectedCityOrigin = "LGN";
            var expectedCityDestination = "MNL";
            var expectedEta = DateTime.Now.AddHours(3);
            var expectedEtd = DateTime.Now.AddHours(4);

            newFlight.CityOfOrigin = expectedCityOrigin;
            newFlight.CityOfDestination = expectedCityDestination;
            newFlight.Eta = expectedEta;
            newFlight.Etd = expectedEtd;

            // Act
            sut.Update(newFlight.FlightId, newFlight);

            // Assert
            var updatedFlight = sut.Retrieve(newFlight.FlightId);
            Assert.AreEqual(expectedCityOrigin, updatedFlight.CityOfOrigin);
            Assert.AreEqual(expectedCityDestination, updatedFlight.CityOfDestination);
            Assert.AreEqual(expectedEta, updatedFlight.Eta);
            Assert.AreEqual(expectedEtd, updatedFlight.Etd);

            //Cleanup
            sut.Delete(updatedFlight.FlightId);
        }
    }
}
