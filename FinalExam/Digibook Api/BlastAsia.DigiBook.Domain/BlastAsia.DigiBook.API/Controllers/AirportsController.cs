using BlastAsia.DigiBook.Domain.Models.Airports;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Airports")]
    public class AirportsController : Controller
    {
        private HttpClient client = new HttpClient();
        private string baseUrl = "https://iatacodes.org/api/v6/airports?api_key=dd6a69c4-9ebb-4df8-a0b3-dc00ad3e3ec1";

        [HttpGet, ActionName("GetAirportsInURL")]
        public async Task<IActionResult> GetAirportsInURL()
        {
            List<Airport> list = new List<Airport>();

            try {
                var stringTask = await client.GetStringAsync(baseUrl);

                if (stringTask != null) {
                    var stringResult = JObject.Parse(stringTask);
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