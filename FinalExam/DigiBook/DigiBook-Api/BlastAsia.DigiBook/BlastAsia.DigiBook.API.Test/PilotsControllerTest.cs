using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.API.Test
{
    [TestClass]
    public class PilotsControllerTest
    {
        private Mock<IPilotService> mockPilotService;
        private Mock<IPilotRepository> mockPilotRepository;
        PilotController sut;
        private Guid existingId;
        private Guid nonExistingId;
        private Pilot pilot;

        [TestInitialize]
        public void Iniatilize()
        {
            pilot = new Pilot
            {

            };

            mockPilotService = new Mock<IPilotService>();
            mockPilotRepository = new Mock<IPilotRepository>();
            sut = new PilotController(mockPilotService.Object, mockPilotRepository.Object);
            existingId = Guid.NewGuid();
            nonExistingId = Guid.Empty;

            mockPilotRepository
                .Setup(pr => pr.Retrieve(existingId))
                .Returns(pilot);

            mockPilotRepository
                .Setup(pr => pr.Retrieve(nonExistingId))
                .Returns<Pilot>(null);


        }

        [TestMethod]
        public void GetPilot_WithNoId_ShouldReturnOkObjectValue()
        {
            //Arrange
            mockPilotRepository
                .Setup(pr => pr.Retrieve())
                .Returns(new List<Pilot>());

            //Act
            var result = sut.GetPilot(null);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetPilot_WithId_ShouldReturnOkObjectValue()
        {
            //Arrange


            //Act
            var result = sut.GetPilot(existingId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void CreatePilot_WithEmptyPilot_ReturnBadRequestResult()
        {
            //Arrange
            pilot = null;

            //Act
            var result = sut.CreatePilot(null);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockPilotService
                .Verify(cr => cr.Save(Guid.Empty, pilot), Times.Never());
        }

        [TestMethod]
        public void CreatePilot_WithValidPilot_ReturnCreatedAtActionResult()
        {

            //Act
            var result = sut.CreatePilot(pilot);

            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));

            mockPilotService
                .Verify(cr => cr.Save(Guid.Empty, pilot), Times.Once());
        }

        [TestMethod]
        public void DeletePilot_WithNoId_ShouldReturnNotFoundResult()
        {
            //Arrange

            //Act
            var result = sut.DeletePilot(nonExistingId);


            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockPilotRepository
                .Verify(cr => cr.Delete(nonExistingId), Times.Never());

        }
        [TestMethod]
        public void DeletePilot_WithId_ShouldReturnNoContentResult()
        {
            //Arrange

            //Act
            var result = sut.DeletePilot(existingId);


            //Assert
            mockPilotRepository
                .Verify(cr => cr.Delete(existingId), Times.Once());

            Assert.IsInstanceOfType(result, typeof(NoContentResult));

        }

        [TestMethod]
        public void UpdatePilot_WithNoExistingPilot_ShouldReturnBadRequestResult()
        {
            //Arrange
            pilot = null;

            //Act
            var result = sut.UpdatePilot(pilot, existingId);

            //Arrange
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockPilotService
                .Verify(cr => cr.Save(existingId, pilot), Times.Never());
        }
        [TestMethod]
        public void UpdatePilot_WithNoId_ShouldReturnNotFoundResult()
        {
            //Arrange


            //Act
            var result = sut.UpdatePilot(pilot, nonExistingId);

            //Arrange
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockPilotService
                .Verify(cr => cr.Save(nonExistingId, pilot), Times.Never());
        }

        [TestMethod]
        public void UpdatePilot_WithExistingPilotAndId_ShouldReturnOkObjectResult()
        {


            //Act
            var result = sut.UpdatePilot(pilot, existingId);

            //Arrange
            mockPilotService
                .Verify(cr => cr.Save(existingId, pilot), Times.Once());


            Assert.IsInstanceOfType(result, typeof(OkObjectResult));


        }

    }
}
