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



        [TestMethod]
        public async Task GetCodeAsync_WithValidUrl_ShouldReturnOkResult()
        {
            var _sut = new AirportController();
            var url = "https://iatacodes.org/api/v6/airports?api_key=dd6a69c4-9ebb-4df8-a0b3-dc00ad3e3ec1";
            // Arrange
            // Act
            var result = await _sut.GetCodesAsync(url);

            // Assert
            var value = (result as OkObjectResult).Value;
            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(result, expectedType: typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetCodeAsync_WithInValidUrl_ShouldReturnBadRequest()
        {
            var _sut = new AirportController();
            var url = "https://iatacsssodes.org/api/v6/airports?api_key=dd6a69c4-9ebb-4df8-a0b3-dc00ad3e3ec1";
            // Arrange
            // Act
            var result = await _sut.GetCodesAsync(url);

            // Assert
            Assert.IsInstanceOfType(result, expectedType: typeof(BadRequestResult));
        }
    }
}
