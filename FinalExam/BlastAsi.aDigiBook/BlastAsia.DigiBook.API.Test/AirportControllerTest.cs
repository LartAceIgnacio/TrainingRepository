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
        public void GetAirports_WithValidUrl_ShouldReturnOkResult()
        {
            // Arrange
            var sut = new AirportController();

            // Act
            var result = sut.GetAirports();

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<IActionResult>));
        }
    }
}
