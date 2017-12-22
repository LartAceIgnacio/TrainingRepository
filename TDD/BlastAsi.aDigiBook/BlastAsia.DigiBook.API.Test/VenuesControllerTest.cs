using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.API.Test
{
    [TestClass]
    public class VenuesControllerTest
    {
        private Venue venue;
        private Mock<IVenueRepository> mockVenueRepository;
        private Mock<IVenueService> mockVenueService;
        private VenuesController sut;
        private Guid existingVenueID = Guid.NewGuid();
        private Guid nonExistingVenueID = Guid.Empty;

        [TestInitialize]
        public void Initialize()
        {
            venue = new Venue
            {
                VenueName = "Orange wall",
                Description = "Must be orange"
            };

            mockVenueRepository = new Mock<IVenueRepository>();
            mockVenueService = new Mock<IVenueService>();
            sut = new VenuesController(mockVenueService.Object, mockVenueRepository.Object);


            mockVenueRepository
               .Setup(c => c.Retrieve(existingVenueID))
               .Returns(venue);
        }

        [TestMethod]
        public void GetVenue_WithEmptyId_ShouldReturnOkObject()
        {
            // Arrange
            mockVenueRepository
                .Setup(v => v.Retrieve())
                .Returns(() => new List<Venue>()
                {
                    new Venue()
                });

            // Act
            var result = sut.GetVenue(null);

            // Assert
            mockVenueRepository.Verify(v => v.Retrieve(), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetVenue_WithExistingId_ShouldReturnOkObjectResult()
        {
            // Arrange
            venue.VenueId = existingVenueID;

            // Act
            var result = sut.GetVenue(venue.VenueId);

            //Assert
            mockVenueRepository.Verify(v => v.Retrieve(venue.VenueId), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }


        [TestMethod]
        public void CreateVenue_WithValidData_ShoulReturnCreatedAtActionResult()
        {
            // Arrange
            venue.VenueId = nonExistingVenueID;

            mockVenueService
                .Setup(v => v.Save(venue.VenueId,venue))
                .Returns(venue);

            // Act
            var result = sut.CreateVenue(venue);

            // Assert
            mockVenueService.Verify(v => v.Save(venue.VenueId, venue), Times.Once());
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }


        [TestMethod]
        public void CreateVenue_WithInvalidData_ShoulReturnBadRequestObjectResult()
        {
            // Arrange
            venue.VenueName = "";

            // Act
            var result = sut.CreateVenue(venue);

            // Assert
            mockVenueService.Verify(v => v.Save(venue.VenueId, venue), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }


        [TestMethod]
        public void CreateVenue_WithBlankData_ShouldReturnBadRequestResult()
        {
            // Arrange
            venue = null;

            // Act
            var result = sut.CreateVenue(venue);

            // Assert
            mockVenueService.Verify(v => v.Save(venue.VenueId, venue), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}
