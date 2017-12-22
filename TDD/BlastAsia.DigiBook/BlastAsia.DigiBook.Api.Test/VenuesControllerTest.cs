using BlastAsia.DigiBook.Api.Controllers;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Api.Test
{
    [TestClass]
    public class VenuesControllerTest
    {
        private Mock<IVenueRepository> mockVenueRepository;
        private Mock<IVenueService> mockVenueService;
        private VenuesController sut;
        private Object result;
        private Guid existingVenueId;
        private Guid nonExisitingVenueId;
        private Venue venue;

        [TestInitialize]
        public void Init()
        {
            mockVenueRepository = new Mock<IVenueRepository>();
            mockVenueService = new Mock<IVenueService>();
            sut = new VenuesController(mockVenueService.Object, mockVenueRepository.Object);

            existingVenueId = Guid.NewGuid();
            nonExisitingVenueId = Guid.Empty;

            venue = new Venue();

            mockVenueRepository
                .Setup(vr => vr.Retrieve(existingVenueId))
                .Returns(() => new Venue());
        }
        
        [TestMethod]
        public void GetVenues_WithEmptyVenueId_ReturnsOkObjectResult()
        {
            //Arrange

            //Act 
            result = sut.GetVenues(null);
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }


        [TestMethod]
        public void GetVenues_WithExistingVenueId_ReturnsOkObjectResult()
        {
            //Arrange
            
            //Act 
            result = sut.GetVenues(existingVenueId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockVenueRepository
                .Verify(vr => vr.Retrieve(existingVenueId), Times.Once);
        }


        [TestMethod]
        public void CreateVenue_WithValidData_ReturnsOkObjectResult()
        {
            //Arrange
            //Act 
            result = sut.CreateVenue(venue);
            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockVenueService
                .Verify(vr => vr.Save(Guid.Empty, venue), Times.Once);

        }


        [TestMethod]
        public void CreateVenue_WithInvalidVenueData_ReturnsBadRequestResult()
        {
            //Arrange
            venue = null;
            //Act 
            result = sut.CreateVenue(venue);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            mockVenueService
                .Verify(vr => vr.Save(Guid.Empty, venue), Times.Once);
        }


        [TestMethod]
        public void DeleteVenue_WithExistingVenueId_ReturnsOkResult()
        {
            //Arrange
            
            //Act 
            result = sut.DeleteVenue(existingVenueId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            mockVenueRepository
                .Verify(vr => vr.Delete(existingVenueId));
        }


        [TestMethod]
        public void DeleteVenue_WithNonExistingVenueId_ReturnsNotFoundResult()
        {
            //Arrange

            //Act 
            result = sut.DeleteVenue(nonExisitingVenueId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockVenueRepository
                .Verify(vr => vr.Delete(existingVenueId), Times.Never);
        }


        [TestMethod]
        public void UpdateVeneue_WithValidVenueDataAndVenueId_ReturnsOkObjectResult()
        {
            //Arrange
          
            //Act 
            result = sut.UpdateVenue(venue, existingVenueId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockVenueService
                 .Verify(vs => vs.Save(existingVenueId, venue), Times.Never);
        }

        [TestMethod]
        public void PatchVenue_WithValidDataAndExistingVenueId_ReturnsOkObjectResult()
        {
            //Arrange

            var patchedVenue = new JsonPatchDocument();
            //Act 
            result = sut.PatchVenue(patchedVenue, existingVenueId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
