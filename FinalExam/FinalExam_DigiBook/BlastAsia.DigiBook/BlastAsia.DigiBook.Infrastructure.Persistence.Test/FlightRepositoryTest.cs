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
        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordInDatabase()
        {
            // Arrange

            var connectionString
                = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";

            var dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            var dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            var sut = new FlightRepository(dbContext);

            var flight = new Flight
            {
                CityOfOrigin = "123",
                CityOfDestination = "123",
                ExpectedTimeOfArrivalDate = DateTime.Today,
                ExpectedTimeOfArrivalTime = new DateTime().TimeOfDay,
                ExpectedTimeOfDepartureDate = DateTime.Today,
                ExpectedTimeOfDepartureTime = new DateTime().TimeOfDay,
                FlightCode = "OOODDDYYMMddNN",
                DateCreated = new Nullable<DateTime>(),
                DateModified = new Nullable<DateTime>()
            };

            // Act

            var newFlight = sut.Create(flight);

            // Assert

            Assert.IsNotNull(newFlight);
            Assert.IsTrue(newFlight.FlightId != Guid.Empty);

            // Cleanup

            sut.Delete(newFlight.FlightId);
        }
    }
}
