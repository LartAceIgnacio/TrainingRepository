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
        private Mock<IFlightRepository> mockFlightRepository;
        private Mock<IFlightService> mockFlightService;
        private FlightsController sut;
        private Guid existingFlightId;
        private Flight flight;
        private Object result;
        private Guid nonExistingFlightId;
        private JsonPatchDocument patchedFlight;

        [TestInitialize]
        public void TestInitialize()
        {
            mockFlightRepository = new Mock<IFlightRepository>();
            mockFlightService = new Mock<IFlightService>();

            sut = new FlightsController(
                mockFlightRepository.Object
                , mockFlightService.Object);

            flight = new Flight();
            patchedFlight = new JsonPatchDocument();

            existingFlightId = Guid.NewGuid();
            nonExistingFlightId = Guid.Empty;

            mockFlightRepository
                .Setup(c => c.Retrieve(existingFlightId))
                .Returns(flight);

            mockFlightRepository
                .Setup(c => c.Retrieve(nonExistingFlightId))
                .Returns<Flight>(null);
        }
        [TestMethod]
        public void GetFlights_WithEmptyFlightId_ReturnsOkObjectResult()
        {
            // Arrange

            // Act

            result = sut.GetFlights(null);

            // Assert

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockFlightRepository
                .Verify(c => c.Retrieve(), Times.Once);
        }
        [TestMethod]
        public void GetFlights_WithExistingFlightId_ReturnsOkObjectResult()
        {
            // Arrange

            // Act

            result = sut.GetFlights(existingFlightId);

            // Assert

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockFlightRepository
                .Verify(c => c.Retrieve(existingFlightId), Times.Once);
        }

        [TestMethod]
        public void CreateFlight_WithExistingFlightId_ReturnsCreatedAtActionResult()
        {
            // Arrange

            // Act

            result = sut.CreateFlight(flight);

            // Assert

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));

            mockFlightService
                .Verify(c => c.Save(Guid.Empty, flight), Times.Once);
        }
        [TestMethod]
        public void CreateFlight_WithValidFlightData_ReturnsCreatedAtActionResult()
        {
            // Arrange

            flight = null;

            // Act

            result = sut.CreateFlight(flight);

            // Assert

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockFlightService
                .Verify(c => c.Save(Guid.Empty, flight), Times.Never);
        }
        [TestMethod]
        public void DeleteFlight_WithExistingFlightId_ReturnsOkResult()
        {
            // Arrange

            flight.FlightId = existingFlightId;

            // Act

            result = sut.DeleteFlight(flight.FlightId);

            // Assert

            Assert.IsInstanceOfType(result, typeof(OkResult));

            mockFlightRepository
                .Verify(c => c.Delete(flight.FlightId), Times.Once);
        }
        [TestMethod]
        public void DeleteFlight_WithNonExixistingFlightId_ReturnsNotFoundResult()
        {
            // Arrange

            flight.FlightId = nonExistingFlightId;

            // Act

            result = sut.DeleteFlight(nonExistingFlightId);

            // Assert

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockFlightRepository
                .Verify(c => c.Delete(flight.FlightId), Times.Never);
        }
        [TestMethod]
        public void UpdateFlight_WithExistingFlightId_ReturnsOkResult()
        {
            // Arrange

            flight.FlightId = existingFlightId;

            // Act

            result = sut.UpdateFlight(flight, flight.FlightId);

            // Assert

            Assert.IsInstanceOfType(result, typeof(OkResult));

            mockFlightRepository
                .Verify(c => c.Retrieve(flight.FlightId), Times.Once);
            
        }
        [TestMethod]
        public void UpdateFlight_WithNonExistingFlightId_ResturnsBadRequest()
        {
            // Arrange

            flight.FlightId = nonExistingFlightId;

            // Act

            result = sut.UpdateFlight(flight, flight.FlightId);

            // Assert

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockFlightRepository
                .Verify(c => c.Retrieve(flight.FlightId), Times.Once);

            mockFlightService
                .Verify(c => c.Save(flight.FlightId, flight), Times.Never);
        }
        [TestMethod]
        public void PatchFlight_WithNoValidFlightData_ReturnsBadRequestResult()
        {
            // Arrange

            patchedFlight = null;

            // Act

            result = sut.PatchFlight(patchedFlight, flight.FlightId);

            // Assert

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockFlightService
                .Verify(c => c.Save(flight.FlightId, flight), Times.Never);
        }
        [TestMethod]
        public void PatchFlight_WithNonExistingFlightId_ReturnsNotFound()
        {
            // Arrange

            flight.FlightId = nonExistingFlightId;

            // Act

            result = sut.PatchFlight(patchedFlight, flight.FlightId);

            // Assert

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
