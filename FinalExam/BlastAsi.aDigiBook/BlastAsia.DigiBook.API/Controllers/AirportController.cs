using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain.Models.Airports;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("DigiBook-web")]
    [Produces("application/json")]
    [Route("api/Airport")]
    public class AirportController : Controller
    {
        [HttpGet, ActionName("GetAirports")]
        public IActionResult GetAirports(string url)
        {
            HttpClient http = new HttpClient();
            List<Airport> result = new List<Airport>();

            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    return BadRequest();
                }

                var stringTask = http.GetStringAsync(url);
                var stringResult = JObject.Parse(stringTask.Result);
                JArray arrResponse = (JArray)stringResult["response"];
                result = arrResponse.ToObject<List<Airport>>();
                return Ok(result);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}