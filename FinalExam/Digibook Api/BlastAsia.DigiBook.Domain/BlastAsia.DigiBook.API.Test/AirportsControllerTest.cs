using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Models.Airports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Test
{
    [TestClass]
    public class AirportsControllerTest
    {
        private AirportsController sut;
        private string baseUrl = "";

        [TestMethod]
        public void GetAirports_WithValidBaseUrl_ReturnsOKObjectResult()
        {
            //Arrange
            baseUrl = "https://iatacodes.org/api/v6/airports?api_key=dd6a69c4-9ebb-4df8-a0b3-dc00ad3e3ec1";
            sut = new AirportsController();

            //Act
            var result = sut.GetAirports(baseUrl);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetAirports_WithNullBaseUrl_ReturnsNoContentResultResult()
        {
            //Arrange
            baseUrl = "";
            sut = new AirportsController();

            //Act
            var result = sut.GetAirports(baseUrl);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void GetAirports_WithInvalidBaseUrl_ReturnsBadRequestObjectResult()
        {
            //Arrange
            baseUrl = "https://api/v6/airports";
            sut = new AirportsController();

            //Act
            var result = sut.GetAirports(baseUrl);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}
