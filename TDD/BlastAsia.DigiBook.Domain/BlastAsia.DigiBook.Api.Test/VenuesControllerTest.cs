using BlastAsia.DigiBook.Api.Controllers;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
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

        //[TestMethod]
        //public void CreateVenue_WithValidVenue_ShouldReturnCreatedAtActionResult()
        //{

        //}
        [TestInitialize]
        public void Initialize()
        {

        }
        [TestMethod]
        public void GetVenue_WithoutId_ShouldReturnOkObjectValue()
        {
            var mockRepo = new Mock<IVenueRepository>();
            var mockService = new Mock<IVenueService>();
            var sut = new VenuesController(mockService.Object, mockRepo.Object);
            var venueList = new List<Venue>(); 
            //GetVenue if without id
            mockRepo
                .Setup(r => r.Retrieve())
                .Returns(venueList);
            //Act
            var result = sut.GetVenues(null);

          

            //Assert
            mockRepo
                .Verify(r => r.Retrieve(), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetVenue_WithExistingId_ShouldReturnOkObjectValue()
        {
            var mockRepo = new Mock<IVenueRepository>();
            var mockService = new Mock<IVenueService>();
            var sut = new VenuesController(mockService.Object, mockRepo.Object);
            var venue = new Venue
            {
                VenueName ="Venue Name",
                Description=""
            };

            var existingId = Guid.NewGuid();
            venue.VenueId = existingId;
            //Act
            mockRepo
                .Setup(r => r.Retrieve(venue.VenueId))
                .Returns(venue);

            var result = sut.GetVenues(venue.VenueId);
            
            mockRepo
                .Verify(r => r.Retrieve(venue.VenueId),Times.Once);
        }

        [TestMethod]
        public void CreateVenue_WithValidData_ShouldReturnCreatedActionResult()
        {
            var mockRepo = new Mock<IVenueRepository>();
            var mockService = new Mock<IVenueService>();

            var sut = new VenuesController(mockService.Object, mockRepo.Object);
            var venue = new Venue();

            var result = sut.CreateContact(venue);
            mockService
                .Verify(r => r.Save(venue.VenueId,venue), Times.Once);

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }
    }

   
}
