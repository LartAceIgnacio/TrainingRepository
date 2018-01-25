using BlastAsia.DigiBook.Api.Controllers;
using BlastAsia.DigiBook.Domain.Models.Pilots;
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
        private Mock<IPilotRepository> mockPilotRepository;
        private Mock<IPilotService> mockPilotService;
        private PilotsController sut;
        private Guid existingPilotId;
        private Guid nonExistingPilotId;
        private Pilot pilot;

        [TestInitialize]
        public void Initialize()
        {
            mockPilotRepository = new Mock<IPilotRepository>();
            mockPilotService = new Mock<IPilotService>();

            pilot = new Pilot
            {
                FirstName = "Christoper",
                MiddleName = "Magdaleno",
                LastName = "Manuel",
                BirthDate = DateTime.Now.AddYears(-25),
                YearsOfExperience = 10,
                DateActivated = DateTime.Today,
            };

            sut = new PilotsController(mockPilotService.Object, mockPilotRepository.Object);

            existingPilotId = Guid.NewGuid();
            nonExistingPilotId = Guid.Empty;

        }

        [TestMethod]
        public void GetPilot_WithEmptyPilotId_ReturnsOkObjectResult()
        {
            //Arrange
            
            var pageNo = 1;
            var numRec = 10;
            var filterValue = "";
            //Act
            var result = sut.GetPilotsWithPagination(pageNo, numRec, filterValue);

            //Assert
            mockPilotRepository
                .Verify(p => p.Fetch(pageNo, numRec, filterValue), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            //Assert.ReferenceEquals

        }

        [TestMethod]
        public void DeletePilot_WithExistingPilotId_ReturnsOkResult()
        {
            //Arrange
            mockPilotRepository
                .Setup(p => p.Retrieve(existingPilotId))
                .Returns(pilot);
            //Act 
            var result = sut.DeletePilot(existingPilotId);
            //Assert
            mockPilotRepository
                .Verify(p => p.Delete(existingPilotId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkResult));

        }


        [TestMethod]
        public void DeletePilot_WithNonExisitingPilotId_ReturnsNotFoundResult()
        {
            //Arrange

            //Act 
            var result = sut.DeletePilot(nonExistingPilotId);
            //Assert
            mockPilotRepository
                .Verify(p => p.Delete(nonExistingPilotId), Times.Never);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }


        [TestMethod]
        public void MyTestMethod()
        {
            //Arrange
            //var result = sut.UpdatePilot(existingPilotId, pilot);
            //Act 

            //Assert
        }
    }
}
