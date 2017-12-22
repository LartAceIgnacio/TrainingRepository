using BlastAsia.DigiBook.Api.Controllers;
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

        [TestMethod]
        public void GetVenues_WithEmptyAppointmentId_ReturnsOkObjectResult()
        {
            //Arrange
            var mockVenueRepository = new Mock<IVenueRepository>();
            var mockVenuService = new Mock<IVenueService>();
            var sut = new VenuesController();

            //Act 
            var result = sut.GetVenue(null);
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }
    }
}
