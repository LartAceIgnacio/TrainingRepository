using BlastAsia.DigiBook.Domain.Exceptions;
using BlastAsia.DigiBook.Domain.Flights;
using BlastAsia.DigiBook.Domain.Flights.Exceptions;
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
                CityOfOrigin = "123",
                CityOfDestination = "123",
                ExpectedTimeOfArrival = DateTime.Now.AddHours(1),
                ExpectedTimeOfDeparture = DateTime.Now.AddHours(1),
                FlightCode = "OOODDDYYMMddNN",
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
        [TestCleanup]
        public void CleanupTest()
        {

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
        [TestMethod]
        public void Save_WithValidData_ReturnsNewFlightWithFlightId()
        {
            // Arrange

            mockFlightRepository
                .Setup(c => c.Create(flight))
                .Callback(() => flight.FlightId = Guid.NewGuid())
                .Returns(flight);

            // Act

            var newFlight = sut.Save(flight.FlightId, flight);

            // Assert

            Assert.IsTrue(newFlight.FlightId != Guid.Empty);
        }
        [TestMethod]
        public void Save_WithCityOfOriginNotEqualToThree_ThrowsMaximumLengthException()
        {
            // Arrange

            flight.CityOfOrigin = "1234";

            // Act

            // Assert

            mockFlightRepository
                .Verify(c => c.Create(flight), Times.Never());
            Assert.ThrowsException<MaximumLengthException>(
                () => sut.Save(flight.FlightId, flight));
        }
        [TestMethod]
        public void Save_WithCityOfDestinationNotEqualToThree_ThrowsMaximumLenghtException()
        {
            // Arrange

            flight.CityOfDestination = "1234";

            // Act

            // Assert

            mockFlightRepository
                .Verify(c => c.Create(flight), Times.Never());
            Assert.ThrowsException<MaximumLengthException>(
                () => sut.Save(flight.FlightId, flight));
        }
        [TestMethod]
        public void Save_WithETAGreaterThanETD_ThrowsDateAndTimeException()
        {
            // Arrange

            flight.ExpectedTimeOfArrival = DateTime.Now.AddHours(2);
            flight.ExpectedTimeOfDeparture = DateTime.Now.AddHours(1);

            // Act

            // Assert

            mockFlightRepository
                .Verify(c => c.Create(flight), Times.Never());
            Assert.ThrowsException<DateAndTimeException>(
                () => sut.Save(flight.FlightId, flight));

        }
    }
}
