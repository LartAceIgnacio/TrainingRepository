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
        public async Task GetAsyncAirportsInfo_ShouldReturnOk()
        {
            var _sut = new AirportController();
            // Arrange
            // Act
            var result = await _sut.GetCodesAsync();

            // Assert
            var value = (result as OkObjectResult).Value;
            Assert.IsNotNull(value);
            Assert.IsInstanceOfType(result, expectedType: typeof(OkObjectResult));
        }
    }
}
