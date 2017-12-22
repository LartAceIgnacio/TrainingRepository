using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
using BlastAsia.DigiBook.Domain.Venues.Service;
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
        Mock<IVenueService> mockVenueService;
        Mock<IVenueRepository> mockVenueRepo;
        private VenuesController sut;
        private Venue venue;

        [TestInitialize]
        public void Initialize()
        {
            mockVenueRepo = new Mock<IVenueRepository>();
            mockVenueService = new Mock<IVenueService>();
            sut = new VenuesController(mockVenueService.Object, mockVenueRepo.Object);

            venue = new Venue()
            {
                VenueId = Guid.NewGuid(),
                VenueName = "Training Room A",
                Description = "This is a sample description of training room A."
            };

            mockVenueRepo.Setup(c => c.Retrieve(venue.VenueId))
                .Returns(venue);

            mockVenueRepo.Setup(x => x.Retrieve())
                .Returns(() => new List<Venue>
                {
                    new Venue()
                });

        }

        [TestMethod]
        [TestProperty("API Test", "VenueController")]
        public void GetVenues_WithValidId_ReturnsOkResult()
        {
            // Act
            var result = sut.GetVenues(venue.VenueId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockVenueRepo.Verify(repo => repo.Retrieve(venue.VenueId));
        }

        [TestMethod]
        [TestProperty("API Test", "VenueController")]
        public void GetContacts_WithNoVenueId_ReturnsOkResult()
        {
            // Act
            var result = sut.GetVenues(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockVenueRepo.Verify(repo => repo.Retrieve()); // returns all data
        }

        [TestMethod]
        [TestProperty("API Test", "VenueController")]
        public void PostVenue_WithValidVenueData_ReturnsCreatedAtActionResult()
        {
            // Arrange
            mockVenueService.Setup(repo => repo.Save(Guid.NewGuid(), venue))
                .Returns(venue);

            // Act
            var result = sut.PostVenue(venue);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockVenueService.Verify(repo => repo.Save(Guid.Empty, venue), Times.Once);
        }

        [TestMethod]
        [TestProperty("API Test", "VenueController")]
        public void PostVenue_WithNullVenue_ReturnsBadRequest()
        {
            // act
            var result = sut.PostVenue(null);

            //assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockVenueService.Verify(service => service.Save(Guid.Empty, venue), Times.Never);
        }

        [TestMethod]
        [TestProperty("API Test", "VenueController")]
        public void PostVenue_WithEmptyVenueName_ReturnsBadRequest()
        {
            // Arrange
            mockVenueService.Setup(repo => repo.Save(Guid.Empty, venue))
                .Throws<Exception>();

            venue.VenueName = string.Empty;

            // Act 
            var result = sut.PostVenue(venue);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            
        }

        [TestMethod]
        [TestProperty("API Test", "VenueController")]
        public void DeleteVenue_WithNotValidVenueId_ReturnNotFound()
        {
            // Arrange
            var guid = Guid.NewGuid();
            // Act
            var result = sut.DeleteVenue(guid);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockVenueRepo.Verify(repo => repo.Delete(guid), Times.Never);
        }

         [TestMethod]
        [TestProperty("API Test", "VenueController")]
        public void DeleteVenue_WithValidId_ReturnsNoContentResult()
        {
            // Act

            var result = sut.DeleteVenue(venue.VenueId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockVenueRepo.Verify(repo => repo.Delete(venue.VenueId), Times.Once);
        }

        [TestMethod]
        [TestProperty("API Test", "VenueController")]
        public void Updatevenue_WithValidData_ReturnsOkResult()
        {
            // Act
            var result = sut.UpdateVenue(venue,venue.VenueId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            mockVenueService.Verify(service => service.Save(venue.VenueId, venue), Times.Once);

        }

        [TestMethod]
        [TestProperty("API Test", "VenueController")]
        public void Updatevenue_WithInvalidName_ReturnsBadRequest()
        {
            // Act
            venue.VenueName = null;

            mockVenueService.Setup(service => service.Save(venue.VenueId, venue))
                .Throws<Exception>();

            // Act
            var result = sut.UpdateVenue(venue, venue.VenueId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockVenueRepo.Verify(repo => repo.Retrieve(venue.VenueId), Times.Once);
            mockVenueService.Verify(service => service.Save(venue.VenueId, venue), Times.Once);
        }

        [TestMethod]
        [TestProperty("API Test", "VenueController")]
        public void Patch_WithNullPatchVenue_ReturnsBadRequest()
        {
            // Arrange

            // Act
            var result = sut.PatchVenue(null, venue.VenueId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockVenueRepo.Verify(repo => repo.Retrieve(venue.VenueId), Times.Never);
            mockVenueService.Verify(service => service.Save(venue.VenueId, venue), Times.Never);
        }

        [TestMethod]
        [TestProperty("API Test", "VenueController")]
        public void Patch_WithoutRetrievedVenue_ReturnsNotFounnd()
        {
            // Arrange
            var patchedDoc = new JsonPatchDocument();
            var guid = Guid.Empty;

            mockVenueService.Setup(service => service.Save(guid, venue))
                .Returns<Venue>(null);

            // Act
            var result = sut.PatchVenue(patchedDoc, guid);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockVenueRepo.Verify(repo => repo.Retrieve(guid), Times.Once);
            mockVenueService.Verify(service => service.Save(guid, venue), Times.Never);
        }
    
    }
}
