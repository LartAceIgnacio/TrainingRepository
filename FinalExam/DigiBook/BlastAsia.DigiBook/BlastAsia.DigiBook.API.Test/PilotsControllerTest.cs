using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Pilots;
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
    public class PilotsControllerTest
    {
        private Pilot pilot;
        private Mock<IPilotRepository> mockPilotRepository;
        private Mock<IPilotService> mockPilotService;
        private JsonPatchDocument patchedPilot;

        private PilotsController sut;

        private Guid existingPilotId = Guid.NewGuid();
        private Guid nonExistingPilotId = Guid.Empty;

        [TestInitialize]
        public void Initialize()
        {
            mockPilotRepository = new Mock<IPilotRepository>();
            mockPilotService = new Mock<IPilotService>();
            patchedPilot = new JsonPatchDocument();

            pilot = new Pilot
            {
                PilotId = Guid.NewGuid()
            };

            sut = new PilotsController(mockPilotService.Object,
                mockPilotRepository.Object);

            mockPilotRepository
                .Setup(d => d.Retrieve())
                .Returns(() => new List<Pilot>{
                    new Pilot() });

            mockPilotRepository
                .Setup(d => d.Retrieve(pilot.PilotId))
                .Returns(pilot);
            mockPilotRepository
                .Setup(d => d.Retrieve(existingPilotId))
                .Returns(pilot);
        }
        [TestMethod]
        public void GetPilots_WithEmptyPilotId_ReturnsOkObjectResult()
        {
            // Act
            var result = sut.GetPilots(null);

            // Assert 
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockPilotRepository
                .Verify(d => d.Retrieve(), Times.Once());
        }
        [TestMethod]
        public void GetPilot_WithPilotId_ReturnsOkObjectResult()
        {
            // Act
            var result = sut.GetPilots(existingPilotId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockPilotRepository
                .Verify(d => d.Retrieve(existingPilotId), Times.Once());
        }
        [TestMethod]
        public void CreatePilot_WithEmptyPilot_ReturnsBadRequestResult()
        {
            // Act 
            pilot = null;
            var result = sut.CreatePilot(pilot);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockPilotService
                .Verify(d => d.Save(Guid.Empty, pilot), Times.Never);
        }
        [TestMethod]
        public void CreatePilot_WithValidData_ReturnsCreatedAtActionResult()
        {

            // Act 
            var result = sut.CreatePilot(pilot);
            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));

            mockPilotService
                .Verify(d => d.Save(Guid.Empty, pilot), Times.Once); 
        }

        [TestMethod]
        public void UpdatePilot_WithValidPilot_ReturnsObjectResult()
        {
            var result = sut.UpdatePilot(pilot,existingPilotId );

            // Assert

            mockPilotRepository
                .Verify(p => p.Retrieve(existingPilotId), Times.Once());

            mockPilotService
                .Verify(p => p.Save(existingPilotId, pilot), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }
        [TestMethod]
        public void UpdatePilot_WithEmptyPilot_ReturnsBadRequestResult()
        {
            pilot = null;
                
            //Act 

            var result = sut.UpdatePilot(pilot, existingPilotId);
            //Assert

            mockPilotService
                .Verify(p => p.Save(existingPilotId, pilot), Times.Never());

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void UpdatePilot_WithEmptyPilotId_ReturnsNotFoundResult()
        {
            var result = sut.UpdatePilot(pilot, nonExistingPilotId);

            // Assert
            mockPilotRepository
                .Verify(c => c.Update(nonExistingPilotId, pilot), Times.Never());

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        [TestMethod]
        public void DeletePilot_WithPilotId_ReturnsNoContentResult()
        {
            // Act
            var result = sut.DeletePilot(existingPilotId);

            //Assert          
            mockPilotRepository
                .Verify(c => c.Delete(existingPilotId), Times.Once());

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void DeletePilot_WithEmptyPilotId_ReturnsNotFound()
        {
            //Act
            var result = sut.DeletePilot(nonExistingPilotId);

            // Assert 
            mockPilotRepository
                .Verify(c => c.Delete(nonExistingPilotId),
                Times.Never());
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        [TestMethod]
        public void PatchPilot_WithValidPatchedPilot_ReturnsObjectResult()
        {
            var result = sut.PatchPilot(patchedPilot, existingPilotId);
            // Assert

            mockPilotRepository
                .Verify(c => c.Retrieve(existingPilotId), Times.Once());

            mockPilotService
                .Verify(c => c.Save(existingPilotId, pilot), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }
        [TestMethod]
        public void PatchPilot_WithEmptyPatchedPilot_ReturnsBadRequestResult()
        {
            patchedPilot = null;
            // Act
            var result = sut.PatchPilot(patchedPilot, existingPilotId);

            // Assert
            mockPilotRepository
               .Verify(c => c.Retrieve(nonExistingPilotId), Times.Never());

            mockPilotService
                .Verify(c => c.Save(nonExistingPilotId, pilot), Times.Never());

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PatchPilot_WithInvalidPilotId_ReturnsNotFound()
        {
            var result = sut.PatchPilot(patchedPilot, nonExistingPilotId);

            //Assert
            mockPilotRepository
                .Verify(c => c.Retrieve(nonExistingPilotId), Times.Once());

            mockPilotService
                 .Verify(c => c.Save(nonExistingPilotId, pilot), Times.Never());

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }


    }
}
