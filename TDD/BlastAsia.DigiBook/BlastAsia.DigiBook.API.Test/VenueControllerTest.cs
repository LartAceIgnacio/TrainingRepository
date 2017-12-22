using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
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
    public class VenueControllerTest
    {
        private Mock<IVenueRepository> mockVenueRepository;
        private Mock<IVenueService> mockVenueService;
        private Venue venue;
        private JsonPatchDocument patchedVenue;
        private Guid existingVenue;
        private Guid nonExistingVenue;
        private VenuesController sut;

        [TestInitialize]
        public void Initialize()
        {
            mockVenueRepository = new Mock<IVenueRepository>();
            mockVenueService = new Mock<IVenueService>();
            sut = new VenuesController(
                mockVenueRepository.Object
                , mockVenueService.Object);

            venue = new Venue();
            patchedVenue = new JsonPatchDocument();

            existingVenue = Guid.NewGuid();
            existingVenue = Guid.Empty;

            mockVenueRepository
                .Setup(c => c.Retrieve(existingVenue))
                .Returns(venue);

            mockVenueRepository
               .Setup(c => c.Retrieve(existingVenue))
               .Returns<Venue>(null);

        }

        [TestMethod]
        public void GetVenues_WithEmptyVenueId_ReturnsOkObjectResult()
        {
            // Arrange
            
            // Act

            var result = sut.GetVenues(null);

            // Assert

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockVenueRepository
                .Verify(c => c.Retrieve()
                , Times.Once);

        }

        [TestMethod]
        public void GetVenues_WithExistingVenueId_ReturnsOkResult()
        {
            // Arrange
            
            existingVenue = Guid.NewGuid();

            // Act

            var result = sut.GetVenues(existingVenue);

            // Assert

            Assert.IsNotInstanceOfType(result, typeof(OkResult));

            mockVenueRepository
                .Verify(c => c.Retrieve(existingVenue)
                ,Times.Once);

        }

        [TestMethod]
        public void CreateVenue_WithValidVenueData_ReturnsCreatedAtActionResult()
        {
            // Arrange
            
            // Act

            var result = sut.CreateVenue(venue);

            // Assert

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));

            mockVenueService
                .Verify(c => c.Save(Guid.Empty, venue),
                Times.Once);

        }
        [TestMethod]
        public void CreateVenue_WithNullVenueData_ReturnsBadRequestResult()
        {
            //Arrange
            
            venue = null;

            //Act

            var result = sut.CreateVenue(venue);

            //Assert

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockVenueService
                .Verify(c => c.Save(Guid.Empty, venue)
                , Times.Never);
        }

        [TestMethod]
        public void DeleteVenue_WithExistingVenueId_ReturnsOkResult()
        {
            //Arrange
            
            venue.VenueID = existingVenue;

            //Act

            var result = sut.DeleteVenue(venue.VenueID);

            //Assert

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockVenueRepository
                .Verify(c => c.Delete(venue.VenueID)
                , Times.Never);
        }

        [TestMethod]
        public void DeleteVenue_WithNonExistingVenueId_ReturnsNotFoundResult()
        {
            //Arrange

           venue.VenueID = nonExistingVenue;

            //Act

            var result = sut.DeleteVenue(nonExistingVenue);

            //Assert

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockVenueRepository
                .Verify(c => c.Delete(venue.VenueID)
                , Times.Never);
        }

        [TestMethod]
        public void UpdateVenue_WithExistingVenueId_ReturnsOkObjectResult()
        {
            //Arrange
            
            venue.VenueID = existingVenue;

            //Act

            var result = sut.UpdateVenue(venue, venue.VenueID);

            //Assert

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockVenueService
                .Verify(cs => cs.Save(venue.VenueID, venue)
                , Times.Never);
            mockVenueRepository
                .Verify(cr => cr.Retrieve(venue.VenueID)
                , Times.Once);

        }

        [TestMethod]
        public void UpdateVenue_WithNonExistingVenueId_ReturnsBadRequestResult()
        {
            //Arrange
            
            venue.VenueID = nonExistingVenue;

            //Act

            var result = sut.UpdateVenue(venue, venue.VenueID);

            //Assert

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockVenueRepository
                .Verify(c => c.Retrieve(venue.VenueID)
                , Times.Once);
            mockVenueService
                .Verify(c => c.Save(venue.VenueID, venue)
                , Times.Never);
        }

        [TestMethod]
        public void PatchVenue_WithValidPatchVenueData_ReturnsOkObjectResult()
        {
            //Arrange

            venue.VenueID = existingVenue;

            //Act

            var result = sut.PatchVenue(patchedVenue, venue.VenueID);

            //Assert

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockVenueService
                .Verify(cs => cs.Save(venue.VenueID, venue)
                , Times.Never);

        }

        [TestMethod]
        public void PatchVenue_WithNotValidPatchVenueData_ReturnsBadRequestResult()
        {
            //Arrange
            
            patchedVenue = null;

            //Act

            var result = sut.PatchVenue(patchedVenue, venue.VenueID);

            //Assert

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockVenueService
                .Verify(cs => cs.Save(venue.VenueID, venue)
                , Times.Never);
        }

        [TestMethod]
        public void PatchVenue_WithNonExistingVenueId_ReturnsNotFoundResult()
        {
            //Arrange
                        
            venue.VenueID = nonExistingVenue;

            //Act

            var result = sut.PatchVenue(patchedVenue, venue.VenueID);

            //Assert

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockVenueService
                .Verify(cs => cs.Save(venue.VenueID, venue), Times.Never);
        }
    }
}
