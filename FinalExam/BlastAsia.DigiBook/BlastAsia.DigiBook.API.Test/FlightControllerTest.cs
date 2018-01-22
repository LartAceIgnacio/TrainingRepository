using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Flights;
using BlastAsia.DigiBook.Domain.Models.Flights;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.API.Test
{
    [TestClass]
    public class FlightControllerTest
    {
        private Mock<IFlightService> mockFlightService;
        private Mock<IFlightRepository> mockFlightRepository;
        private FlightsController sut;
        private Flight flight;
        private JsonPatchDocument patchedFlight;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            flight = new Flight
            {
                CityOfOrigin = "MNL",
                CityOfDestination = "LGN",
                Eta = DateTime.Now.AddHours(2),
                Etd = DateTime.Now.AddHours(3)
            };

            mockFlightService = new Mock<IFlightService>();
            mockFlightRepository = new Mock<IFlightRepository>();

            sut = new FlightsController(mockFlightService.Object, mockFlightRepository.Object);

            patchedFlight = new JsonPatchDocument();
            patchedFlight.Replace("CityOfOrigin", "QCC");

            mockFlightRepository
                .Setup(f => f.Retrieve(flight.FlightId))
                .Returns(flight);
        }

        [TestMethod]
        public void GetFlights_WithEmptyFlightId_ReturnOkObjectResult()
        {           
            // Arrange
            mockFlightRepository
                .Setup(f => f.Retrieve())
                .Returns(new List<Flight>());

            // Act
            var result = sut.GetFlights(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockFlightRepository.Verify(f => f.Retrieve(), Times.Once);
        }

        [TestMethod]
        public void GetFlights_WithValidFlightId_ReturnOkObjectResult()
        {
            // Act
            var result = sut.GetFlights(flight.FlightId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockFlightRepository.Verify(f => f.Retrieve(flight.FlightId), Times.Once);
        }

        [TestMethod]
        public void CreateFlight_FlightWithValidDetails_ReturnCreatedAtActionResult()
        {
            // Act
            var result = sut.CreateFlight(flight);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockFlightService.Verify(f => f.Save(Guid.Empty, flight), Times.Once);
        }

        [TestMethod]
        public void CreateFlight_FlightWithEmptyData_ReturnBadRequestResult()
        {
            // Arrange
            flight = null;

            // Act
            var result = sut.CreateFlight(flight);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockFlightService.Verify(f => f.Save(Guid.Empty, flight), Times.Never);
        }

        [TestMethod]
        public void DeleteFlight_FlightDeleted_ReturnNoContentResult()
        {
            // Act
            var result = sut.DeleteFlight(flight.FlightId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockFlightRepository.Verify(f => f.Delete(flight.FlightId), Times.Once);
        }

        [TestMethod]
        public void DeleteFlight_WithoutFlightId_ReturnNotFoundResult()
        {
            // Arrange
            mockFlightRepository
                .Setup(f => f.Retrieve(flight.FlightId))
                .Returns<Flight>(null);

            // Act
            var result = sut.DeleteFlight(flight.FlightId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockFlightRepository.Verify(f => f.Delete(flight.FlightId), Times.Never);
        }

        [TestMethod]
        public void UpdateFlight_WithExistingFlightDataAndId_ReturnOkObjectResult()
        {
            // Act
            var result = sut.UpdateFlight(flight, flight.FlightId);

            // Assert
            mockFlightRepository.Verify(f => f.Retrieve(flight.FlightId), Times.Once);
            mockFlightService.Verify(f => f.Save(flight.FlightId, flight), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void UpdateFlight_WithoutValue_ReturnBadRequestResult()
        {
            // Arrange
            flight = null;

            // Act
            var result = sut.UpdateFlight(flight, Guid.Empty);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockFlightRepository.Verify(f => f.Retrieve(Guid.Empty), Times.Never);
            mockFlightService.Verify(f => f.Save(Guid.Empty, flight), Times.Never);
        }

        [TestMethod]
        public void UpdateFlight_WithNoExistingFlight_ReturnNotFoundResult()
        {
            // Arrange
            flight.FlightId = Guid.Empty;
            mockFlightRepository
                .Setup(f => f.Retrieve(flight.FlightId))
                .Returns<Flight>(null);


            // Act
            var result = sut.UpdateFlight(flight, flight.FlightId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockFlightRepository.Verify(f => f.Retrieve(flight.FlightId), Times.Once);
            mockFlightService.Verify(f => f.Save(flight.FlightId, flight), Times.Never);
        }

        [TestMethod]
        public void PatchFlight_WithExistingFlightDataAndId_ReturnOkObjectResult()
        {
            // Act
            var result = sut.PatchFlight(patchedFlight, flight.FlightId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockFlightRepository.Verify(f => f.Retrieve(flight.FlightId), Times.Once);
            mockFlightService.Verify(f => f.Save(flight.FlightId, flight), Times.Once);
        }

        [TestMethod]
        public void PatchFlight_WithoutExistingFlightDataAndId_ReturnNotFoundResult()
        {
            // Arrange
            mockFlightRepository
                .Setup(f => f.Retrieve(flight.FlightId))
                .Returns<Flight>(null);

            // Act
            var result = sut.PatchFlight(patchedFlight, flight.FlightId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockFlightRepository.Verify(f => f.Retrieve(flight.FlightId), Times.Once);
            mockFlightService.Verify(f => f.Save(flight.FlightId, flight), Times.Never);
        }

        [TestMethod]
        public void PatchFlight_FlightWithEmptyPatchDocument_ReturnBadRequest()
        {
            // Arrange
            patchedFlight = null;

            // Act
            var result = sut.PatchFlight(patchedFlight, flight.FlightId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockFlightRepository.Verify(f => f.Retrieve(flight.FlightId), Times.Never);
            mockFlightService.Verify(f => f.Save(flight.FlightId, flight), Times.Never);
        }
    }
}
