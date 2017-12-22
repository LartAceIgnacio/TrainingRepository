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
        VenuesController sut;

        [TestInitialize]
        public void Initialize()
        {
            venue = new Venue
            {
                VenueId = Guid.NewGuid(),
                VenueName = "Venue",
                Description = "Description"
            };

            mockVenueRepository = new Mock<IVenueRepository>();
            mockVenueService = new Mock<IVenueService>();

            sut = new VenuesController(mockVenueRepository.Object, mockVenueService.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void GetVenues_WithoutVenueId_ReturnOkObjectResult()
        {
            // Act
            var result = sut.GetVenues(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockVenueRepository.Verify(v => v.Retrieve(), Times.Once);

        }

        [TestMethod]
        public void GetVenues_WithVenueId_ReturnOkObjectResult()
        {
            // Arrange
            mockVenueRepository
                .Setup(v => v.Retrieve(venue.VenueId))
                .Returns(venue);

            // Act
            var result = sut.GetVenues(venue.VenueId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockVenueRepository.Verify(v => v.Retrieve(venue.VenueId), Times.Once);
        }

        [TestMethod]
        public void CreateVenue_VenueWithValidData_ReturnCreatedAtActionResult()
        {
            // Act
            var result = sut.CreateVenue(venue);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockVenueService.Verify(v => v.Save(venue), Times.Once);
        }

        //[TestMethod]
        //public void ()
        //{
        //    // Arrange

        //    // Act

        //    // Assert

        //}
    }
}
