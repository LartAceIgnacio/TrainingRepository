using BlastAsia.DigiBook.Domain.Flights;
using BlastAsia.DigiBook.Domain.Models.Flights;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.Flights
{
    [TestClass]
    public class FlightServiceTest
    {
        [TestMethod]
        public void Create_NewFlightWithValidData_ShouldCallRepositoryCreate()
        {
            // Arrange

            var mockFlightRepository = new Mock<IFlightRepository>();
            var sut = new FlightService(mockFlightRepository.Object);

            var flight = new Flight
            {
                CityOfOrigin = "",
                CityOfDestination = "",
                ExpectedTimeOfArrival = new DateTime(),
                ExpectedTimeOfDeparture = new DateTime(),
                FlightCode = "",
                DateCreated = new Nullable<DateTime>(),
                DateModified = new Nullable<DateTime>()

            };

            var nonExistingFlightId = Guid.Empty;

            mockFlightRepository
                .Setup(c => c.Retrieve(nonExistingFlightId))
                .Returns<Flight>(null);

            // Act

            var result = sut.Save(flight.FlightId, flight);

            // Assert

            mockFlightRepository
                .Verify(c => c.Retrieve(nonExistingFlightId), Times.Once);
            mockFlightRepository
                .Verify(c => c.Create(flight), Times.Once);
        }
    }
}
