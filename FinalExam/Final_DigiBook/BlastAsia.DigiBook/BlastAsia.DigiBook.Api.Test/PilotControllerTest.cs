using BlastAsia.DigiBook.Api.Controllers;
using BlastAsia.DigiBook.Domain.Pilots;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Api.Test
{
    [TestClass]
    public class PilotControllerTest
    {
        [TestMethod]
        public void GetPilot_WithEmptyPilotId_ReturnsOkObjectResult()
        {
            //Arrange
            var mockPilotRepository = new Mock<IPilotRepository>();
            var mockPilotService = new Mock<IPilotService>();

            var sut = new PilotsController(mockPilotService.Object, mockPilotRepository.Object);
            //Act
            var result = sut.Get();

            //Assert
            mockPilotRepository
                .Verify(c => c.Retrieve(), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            //Assert.ReferenceEquals

        }
    }
}
