using BlastAsia.DigiBook.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.Api.Test
{
    [TestClass]
    public class AirportControllerTest
    {
        string url = "https://iatacodes.org/api/v6/airports?api_key=dd6a69c4-9ebb-4df8-a0b3-dc00ad3e3ec1";
        private AirportsController sut;

        [TestInitialize]
        public void Initialize()
        {

            sut = new AirportsController();
        }
        [TestMethod]
        public async Task GetAirports_AllData_ShouldReturnOkObjectValue()
        {
            var result = await sut.GetAirports("",url);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetAirports_WithInvalidUrl_ShoulReturnBadRequest()
        {
            var result = await sut.GetAirports("", "");
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }

    
}
