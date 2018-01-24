using BlastAsia.DigiBook.Domain.Models.Airports;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Controllers
{
    [EnableCors("Day2App")]
    [Produces("application/json")]
    [Route("api/Airports")]
    public class AirportsController : Controller
    {
        private HttpClient client = new HttpClient();

        [HttpGet, ActionName("GetAirports")]
        public IActionResult GetAirports(string baseUrl)
        {
            List<Airport> list = new List<Airport>();

            try {
                if (!string.IsNullOrEmpty(baseUrl)) {
                    var stringTask = client.GetStringAsync(baseUrl);
                    var stringResult = JObject.Parse(stringTask.Result);
                    JArray arrResponse = (JArray)stringResult["response"];
                    list = arrResponse.ToObject<List<Airport>>();
                    return Ok(list);
                }

                return NoContent();
            }
            catch {
                return BadRequest();
            }
        }
    }
}