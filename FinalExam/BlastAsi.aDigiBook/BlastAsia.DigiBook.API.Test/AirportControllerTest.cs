using BlastAsia.DigiBook.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Test
{
    [TestClass]
    public class AirportControllerTest
    {
        private AirportController sut;
        private string url;

        [TestInitialize]
        public void Initialize()
        {
            sut = new AirportController();
            url = "https://iatacodes.org/api/v6/airports?api_key=dd6a69c4-9ebb-4df8-a0b3-dc00ad3e3ec1";

        }


        [TestCleanup]
        public void CleanUp()
        {
        }

        [TestMethod]
        public void GetAirports_WithValidUrl_ShouldReturnOkObjectResult()
        {
            // Arrange

            // Act
            var result = sut.GetAirports(url);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetAirports_WithBlankUrl_ShouldReturnNoContentResult()
        {
            // Arrange
            url = null;

            // Act
            var result = sut.GetAirports(url);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void GetAirports_WithInValidUrl_ShouldReturnBadRequestResult()
        {
            // Arrange
            url = "https://iatacodes.org/api/v6/airports?api_key=dd6a69c4-9ebb-4df8-a0b3-dc00ad3e3ec";

            // Act
            var result = sut.GetAirports(url);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}
