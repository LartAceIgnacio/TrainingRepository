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
    public class VenueControllerTest : Controller
    {
        private Venue venue;
        private Mock<IVenueRepository> mockVenueRepository;
        private Mock<IVenueService> mockVenueService;
        private JsonPatchDocument patchedVenue;
        private VenuesController sut;

        private Guid existingVenueId = Guid.NewGuid();
        private Guid notExistingVenueId = Guid.Empty;
        
        [TestInitialize]
        public void Initialize()
        {
            mockVenueRepository = new Mock<IVenueRepository>();
            mockVenueService = new Mock<IVenueService>();
            patchedVenue = new JsonPatchDocument();

            venue = new Venue
            {

                VenueId = Guid.NewGuid()
            };

            sut = new VenuesController(mockVenueService.Object,
               mockVenueRepository.Object);

            mockVenueRepository
               .Setup(v => v.Retrieve())
               .Returns(() => new List<Venue>{
                   new Venue()});

            mockVenueRepository
                .Setup(v => v.Retrieve(venue.VenueId))
                .Returns(venue);

            mockVenueRepository
                .Setup(v => v.Retrieve(existingVenueId))
                .Returns(venue);

            mockVenueRepository
                .Setup(d => d.Retrieve(notExistingVenueId))
                .Returns<Venue>(null);
        }

        [TestMethod]
        public void GetVenue_WithEmptyVenueId_ReturnsOkObjectResult()
        {
            // Act
            var result = sut.GetVenue(null);
           
            //Assert

            mockVenueRepository
               .Verify(c => c.Retrieve(), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetVenue_WithVenueId_ReturnsOkObjectResult()
        {
            // Act 
            var result = sut.GetVenue(existingVenueId);

            // Assert
            mockVenueRepository
               .Verify(c => c.Retrieve(existingVenueId), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void CreateVenue_WithEmptyVenue_ReturnsBadRequestResult()
        {
            //Arrange
            venue = null;

            //Act 

            var result = sut.CreateVenue(venue);

            //Assert 

            mockVenueService
              .Verify(c => c.Save(notExistingVenueId, venue), Times.Never());

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

        }
        [TestMethod]
        public void CreateVenue_WithValidVenue_ReturnsCreatedAtActionResult()
        {
            //Act 
            mockVenueService
                .Setup(v => v.Save(notExistingVenueId, venue))
                .Returns(venue);

            var result = sut.CreateVenue(venue);

            //Assert 

            mockVenueService
              .Verify(c => c.Save(notExistingVenueId, venue), Times.Once());

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void UpdateVenue_WithEmptyVenue_ReturnsBadRequestResult()
        {
            //Act
            venue = null;
            var result = sut.UpdateVenue(venue,existingVenueId);

            //Assert 

            mockVenueRepository
              .Verify(v => v.Retrieve(existingVenueId), Times.Never());

            mockVenueService
              .Verify(v => v.Save(existingVenueId,venue), Times.Never());

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void UpdateVenue_WithEmptyVenueId_ReturnsNotFound()
        {
            // Act
            var result = sut.UpdateVenue(venue, notExistingVenueId);

            //Assert
            mockVenueRepository
             .Verify(v => v.Update(notExistingVenueId, venue), Times.Never());

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        [TestMethod]
        public void UpdateVenue_WithVenueId_ReturnsOkObjectResult()
        {
            //Act
            var result = sut.UpdateVenue(venue, existingVenueId);

            //Assert

            mockVenueRepository
            .Verify(v => v.Retrieve(existingVenueId), Times.Once());

            mockVenueService
           .Verify(v => v.Save(existingVenueId, venue), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void DeleteVenue_WithEmptyDeleteId_ReturnsNotFoundResult()
        {
            //Act 
            var result = sut.DeleteVenue(notExistingVenueId);

            //Assert
            mockVenueRepository
                .Verify(v => v.Delete(notExistingVenueId),Times.Never());

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        [TestMethod]
        public void DeleteVenue_WithVenueId_ReturnsNoContentResult()
        {
            //Act
            var result = sut.DeleteVenue(existingVenueId);

            //Assert
            mockVenueRepository
                .Verify(v => v.Delete(existingVenueId), Times.Once());

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void PatchVenue_WithEmptyVenue_ReturnsBadRequestResult()
        {
            //Arrange
            patchedVenue = null;
            // Act
            var result = sut.PatchVenues(patchedVenue,existingVenueId);

            //Assert
            mockVenueRepository
                .Verify(v => v.Retrieve(existingVenueId), Times.Never());

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PatchVenue_WithVenueId_ReturnsOkObjectResult()
        {
            //Act

            var result = sut.PatchVenues(patchedVenue, existingVenueId);

            //Assert
            mockVenueRepository
                .Verify(v => v.Retrieve(existingVenueId), Times.Once());

            mockVenueService
                .Verify(v => v.Save(existingVenueId, venue), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PatchVenue_WithInvalidVenueId_ReturnsNotFoundResult()
        {
            //Act
            var result = sut.PatchVenues(patchedVenue,notExistingVenueId);

            //Assert
            mockVenueRepository
                .Verify(v => v.Retrieve(notExistingVenueId), Times.Once());

            mockVenueService
                .Verify(v => v.Save(notExistingVenueId, venue),Times.Never());

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }


    }
}
