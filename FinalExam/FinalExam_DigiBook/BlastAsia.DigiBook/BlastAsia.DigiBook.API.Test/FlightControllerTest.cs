using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Flights;
using BlastAsia.DigiBook.Domain.Models.Flights;
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
        [TestMethod]
        public void GetFlights_WithEmptyFlightId_ReturnsOkObjectResult()
        {
            // Arrange

            var mockFlightRepository = new Mock<IFlightRepository>();
            var mockFlightService = new Mock<IFlightService>();
            var sut = new FlightsController(
                mockFlightRepository.Object
                , mockFlightService.Object);

            var flight = new Flight();

            // Act

            var result = sut.GetFlights(null);

            // Assert

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockFlightRepository
                .Verify(c => c.Retrieve(), Times.Once);
        }
    }
}
