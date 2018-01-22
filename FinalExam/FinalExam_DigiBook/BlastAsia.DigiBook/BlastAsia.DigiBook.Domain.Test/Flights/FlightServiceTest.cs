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
        private Mock<IFlightRepository> mockFlightRepository;
        private FlightService sut;
        private Flight flight;
        private Guid existingFlightId = Guid.NewGuid();
        private Guid nonExistingFlightId = Guid.Empty;

        [TestInitialize]
        public void InitializeTest()
        {
            flight = new Flight
            {
                CityOfOrigin = "",
                CityOfDestination = "",
                ExpectedTimeOfArrival = new DateTime(),
                ExpectedTimeOfDeparture = new DateTime(),
                FlightCode = "",
                DateCreated = new Nullable<DateTime>(),
                DateModified = new Nullable<DateTime>()

            };

            mockFlightRepository = new Mock<IFlightRepository>();

            sut = new FlightService(mockFlightRepository.Object);

            mockFlightRepository
                .Setup(c => c.Retrieve(existingFlightId))
                .Returns(flight);

            mockFlightRepository
                .Setup(c => c.Retrieve(nonExistingFlightId))
                .Returns<Flight>(null);
        }
        [TestMethod]
        public void Create_NewFlightWithValidData_ShouldCallRepositoryCreate()
        {
            // Arrange

            // Act

            sut.Save(flight.FlightId, flight);

            // Assert

            mockFlightRepository
                .Verify(c => c.Retrieve(nonExistingFlightId), Times.Once);
            mockFlightRepository
                .Verify(c => c.Create(flight), Times.Once);
        }
        [TestMethod]
        public void Update_WithExistingFlight_ShouldCallRepositoryUpdate()
        {
            // Arrange

            flight.FlightId = existingFlightId;

            // Act

            sut.Save(flight.FlightId, flight);

            // Assert

            mockFlightRepository
                .Verify(c => c.Retrieve(existingFlightId), Times.Once);
            mockFlightRepository
                .Verify(c => c.Update(existingFlightId, flight), Times.Once);
        }
    }
}
