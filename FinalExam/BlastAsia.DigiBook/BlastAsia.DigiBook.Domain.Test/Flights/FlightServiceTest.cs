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
        private Flight flight;
        private Mock<IFlightRepository> mockFlightRepository;
        private FlightService sut;

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
            mockFlightRepository = new Mock<IFlightRepository>();
            sut = new FlightService(mockFlightRepository.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void Save_FlightWithValidData_ShouldCallRepositoryCreate()
        {
            // Act
            sut.Save(flight.FlightId, flight);

            // Assert
            mockFlightRepository.Verify(f => f.Create(flight), Times.Once);
        }

        [TestMethod]
        public void Save_FlightWithValidData_ShouldReturnDataWithId()
        {
            // Arrange
            mockFlightRepository
                .Setup(f => f.Create(flight))
                .Callback(() => flight.FlightId = Guid.NewGuid())
                .Returns(flight);

            // Act
            sut.Save(flight.FlightId, flight);

            // Assert
            mockFlightRepository.Verify(f => f.Create(flight), Times.Once);
        }

        [TestMethod]
        public void Save_FlightWithValidData_ShouldReturnFlightCode()
        {
            // Act
            sut.Save(flight.FlightId, flight);

            // Assert
            Assert.IsTrue(flight.FlightCode != null);
            mockFlightRepository.Verify(f => f.Create(flight), Times.Once);
        }

        [TestMethod]
        public void Save_ExistingFlightWithValidData_ShouldCallRepositoryUpdate()
        {
            // Arrange
            mockFlightRepository
                .Setup(f => f.Retrieve(flight.FlightId))
                .Returns(flight);

            // Act
            sut.Save(flight.FlightId, flight);

            // Assert
            mockFlightRepository.Verify(f => f.Retrieve(flight.FlightId), Times.Once);
            mockFlightRepository.Verify(f => f.Update(flight.FlightId, flight), Times.Once);
        }

        [TestMethod]
        public void Save_WithBlankCityOrigin_ShouldCallCityOfOriginRequiredException()
        {
            // Arrange
            flight.CityOfOrigin = null;

            // Assert
            Assert.ThrowsException<CityOfOriginRequiredException>(
                () => sut.Save(flight.FlightId, flight));
            mockFlightRepository.Verify(f => f.Create(flight), Times.Never);
        }

        [TestMethod]
        public void Save_CityOriginLessThanFixedLength_ShouldReturnCityOriginFixedLengthException()
        {
            // Arrange
            flight.CityOfOrigin = "MN";

            // Assert
            Assert.ThrowsException<CityOriginFixedLengthException>(
                () => sut.Save(flight.FlightId, flight));
            mockFlightRepository.Verify(f => f.Create(flight), Times.Never);
        }

        [TestMethod]
        public void Save_CityOriginMoreThanFixedLength_ShouldReturnCityOriginFixedLengthException()
        {
            // Arrange
            flight.CityOfOrigin = "MNLA";

            // Assert
            Assert.ThrowsException<CityOriginFixedLengthException>(
                () => sut.Save(flight.FlightId, flight));
            mockFlightRepository.Verify(f => f.Create(flight), Times.Never);
        }

        [TestMethod]
        public void Save_CityOriginAndCityDestinationTheSame_ShouldReturnDestinationErrorException()
        {
            // Arrange
            flight.CityOfOrigin = flight.CityOfDestination;

            //Assert
            Assert.ThrowsException<DestinationErrorException>(
                () => sut.Save(flight.FlightId, flight));
        }

        [TestMethod]
        public void Save_CityOfDestinationLessThanFixedLength_ShouldThrowCityDestinationFixedLengthException()
        {
            // Arrange
            flight.CityOfDestination = "LG";

            // Assert
            Assert.ThrowsException<CityDestinationFixedLengthException>(
                () => sut.Save(flight.FlightId, flight));
            mockFlightRepository.Verify(f => f.Create(flight), Times.Never);
        }

        [TestMethod]
        public void Save_CityOfDestinationMoreThanFixedLength_ShouldThrowCityDestinationFixedLengthException()
        {
            // Arrange
            flight.CityOfDestination = "LGNA";

            // Assert
            Assert.ThrowsException<CityDestinationFixedLengthException>(
                () => sut.Save(flight.FlightId, flight));
            mockFlightRepository.Verify(f => f.Create(flight), Times.Never);
        }

        [TestMethod]
        public void Save_ArrivalTimeTheSameWithDeparture_ShouldThrowInclusiveArrivalAndDepartureTimeException()
        {
            // Arrange
            flight.Eta = flight.Etd;

            // Assert
            Assert.ThrowsException<InclusiveArrivalAndDepartureTimeException>(
                () => sut.Save(flight.FlightId, flight));
        }

        [TestMethod]
        public void Save_ArrivalTimeLaterThanDeparture_ShouldThrowInclusiveArrivalAndDepartureTimeException()
        {
            // Arrange
            flight.Eta = DateTime.Now.AddHours(5);

            // Assert
            Assert.ThrowsException<InclusiveArrivalAndDepartureTimeException>(
                () => sut.Save(flight.FlightId, flight));
        }

        [TestMethod]
        public void Save_DepartureEarlierThanArrivalTime_ShouldThrowInclusiveArrivalAndDepartureTimeException()
        {
            // Arrange
            flight.Etd = DateTime.Now.AddHours(1);

            // Assert
            Assert.ThrowsException<InclusiveArrivalAndDepartureTimeException>(
                () => sut.Save(flight.FlightId, flight));
        }
    }
}
